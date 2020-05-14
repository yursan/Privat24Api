<template>
  <div id="app">
    <header>
      <h1>{{msg}}</h1>
    </header>
    <main>
      <aside class="sidebar">
        <div v-for="currencyRate in currencyRatesUSD"
        v-bind="currencyRate.id">
          {{currencyRate.date}} : {{currencyRate.saleRatePB}}
        </div>
      </aside>
      <div class="content">
        <div id="chart"></div>
      </div>
    </main>
  </div>
</template>
<script src="https://code.jquery.com/jquery-3.5.1.js">
</script>
<script>
import axios from 'axios'
import d3 from 'metrics-graphics'

export default {
  name: 'app',
  data () {
    return {
      msg: 'Privat24 vue.js SPA',
      currencyRatesUSD: null,
      currencyRatesEUR: null,
      ratesEndpoint: 'api/v1/currencyRates',
    }
  },

  async created() {
    await this.getAllCurrencyRates();
    this.buildLineChart(this.currencyRatesEUR, this.currencyRatesUSD);
  },

  methods: {
    async getAllCurrencyRates() {
      return axios.get(this.ratesEndpoint)
        .then(response => {
          console.log('Response:');
          console.log(response);
          this.currencyRatesUSD = response.data
                .filter(c=> c.currency == 'USD')
                .map(entry => ({
                  date: new Date(entry.date).getFullYear() + '-' + (new Date(entry.date).getMonth() + 1) + '-' + new Date(entry.date).getDate(),
                  saleRatePB: entry.saleRatePB,
                  saleRateNBU: entry.saleRateNBU,
           }));;
          this.currencyRatesEUR = response.data
                .filter(c=> c.currency == 'EUR')
                .map(entry => ({
                  date: new Date(entry.date).getFullYear() + '-' + (new Date(entry.date).getMonth() + 1) + '-' + new Date(entry.date).getDate(),
                  saleRatePB: entry.saleRatePB,
                  saleRateNBU: entry.saleRateNBU,
           }));;

        })
        .catch(error => {
          console.log('-----error-------');
          console.log(error);
        })
    },
    setActivityData(data) {
      console.log('data.length:' + data.length);
      for (var i = 0; i < data.length; i++) {
        console.log('data[i].length:' + data[i].length);
        for (var j = 0; j < data[i].length; j++) {
            if (i === 0) {
                data[i][j].active = (j % 4 === 0);
            }
            if (i === 1) {
                data[i][j].active = (j % 3 === 0);
            }
        }
      }
    },

    buildLineChart(data, data2) {
      console.log('building chart:');
      const chartData = [
        MG.convert.date(data, 'date'), 
        MG.convert.date(data2, 'date')];

      this.setActivityData(chartData);
      console.log(chartData);

      MG.data_graphic({
          title: 'USD Currency Rates',
          area: false,
          animate_on_load: true,
          data: chartData,
          width: 1200,
          height: 350,
          target: '#chart', // the html element that the graphic is inserted in

          x_accessor: 'date',  // the key that accesses the x value
          y_accessor: 'saleRatePB', // the key that accesses the y value
          y_label: 'sales rate',
          right: 40,
          xax_count: 4,
          color: 'green',

        point_size: 4,
        active_point_on_lines: true,
        active_point_accessor: 'active',
        active_point_size: 2,
        aggregate_rollover: true
/*
          mouseover: function(d, i) {
            let date = new Date(d.date);
            let y_val = (d.saleRatePB === 0 || d.saleRatePB === null) ? 'no data' : d.saleRatePB;
            //MG.select('#chart svg .mg-active-datapoint').text(date +  '   ' + y_val);
        }
        */
      });

    }
  }
}
</script>

<style lang="scss">
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}

h1, h2 {
  font-weight: normal;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  display: inline-block;
  margin: 0 10px;
}

a {
  color: #42b983;
}
</style>
