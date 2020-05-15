import Vue from 'vue'
import 'babel-polyfill'
import App from './App.vue'

const app = new Vue({
  el: '#app',
  render: h => h(App)
})
