<script setup lang="ts">
import { ref, onUnmounted } from 'vue';
import SignalrService from './signalrService'

const socket = new SignalrService("http://localhost:40001/ws/").start();

const command1BtnColor = ref('');
const command2BtnColor = ref('');

function sendCommand(command: string) {
  socket.SendCommand(command);
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

onUnmounted(() => {
  socket.stop();
})
</script>

<template>
  <div>
    <button @click="sendCommand('command1')" :style="{ backgroundColor: command1BtnColor }">Command1</button>
    <button @click="sendCommand('command2')" :style="{ backgroundColor: command2BtnColor }">Command2</button>
  </div>
</template>

<style scoped></style>
