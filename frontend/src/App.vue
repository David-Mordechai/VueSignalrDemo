<script setup lang="ts">
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { ref, onUnmounted } from 'vue';

const btnColor = ref('');

const connection = new HubConnectionBuilder()
  .withUrl("http://localhost:40001/ws/", {
    withCredentials: false
  })
  .withAutomaticReconnect()
  .configureLogging(LogLevel.Information)
  .build();

connection.on("messageReceived", (color) => {
  console.log(color);
  btnColor.value = color
});

console.log(connection.state);

connection.start().catch((err) => console.log(err));

function command1() {
  console.log('command1 clicked');
  connection.send('command1');
}

onUnmounted(() => {
  connection.stop();
})
</script>

<template>
  <div>
    <button @click="command1" :style="{ backgroundColor: btnColor }">Command1</button>
  </div>
</template>

<style scoped></style>
