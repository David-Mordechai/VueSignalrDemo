<script setup lang="ts">
import { ref, onUnmounted, onMounted } from 'vue';
import CommunicationService from './CommunicationService'

const command1BtnColor = ref('');
const command2BtnColor = ref('');
const isSocketConnected = ref<boolean>(false);
let socket: CommunicationService;

onMounted(async () => {
  socket = new CommunicationService("http://localhost:40001/ws/");

  socket.ConnectionState = (connected: boolean) => {
    isSocketConnected.value = connected;
  }

  socket.ReceiveReport = (color: string, command: string) => {
    switch (command) {
      case 'command1':
        command1BtnColor.value = color;
        break;
      case 'command2':
        command2BtnColor.value = color;
        break;
      default:
        break;
    }
  }

  await socket.start();
});

onUnmounted(async () => {
  await socket.stop();
})

function sendCommand(command: string) {
  socket.SendCommand(command);
}
</script>

<template>
  <div class="parent">
    <div class="status" v-if="isSocketConnected === true" style="background-color: green;">Connected to server</div>
    <div class="status" v-if="isSocketConnected === false" style="background-color: red;">Disconnected from server</div>
    <button class="cmd1" @click="sendCommand('command1')" :style="{ backgroundColor: command1BtnColor }"
      v-bind:disabled="!isSocketConnected">Command1</button>
    <button class="cmd2" @click="sendCommand('command2')" :style="{ backgroundColor: command2BtnColor }"
      v-bind:disabled="!isSocketConnected">Command2</button>
  </div>
</template>

<style scoped>
.parent {
  display: grid;
  grid-gap: 5px;
  grid-template-areas:
    "status status "
    "cmd1 cmd2";
}

.status {
  grid-area: status;
}

.cmd1 {
  grid-area: cmd1;
}

.cmd2 {
  grid-area: cmd2;
}
</style>