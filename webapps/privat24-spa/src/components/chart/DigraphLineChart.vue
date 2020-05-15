<template>
<div>
    <div id="graphLineChart"></div>
</div>
</template>

<script>
import Dygraph from 'dygraphs';

export default {
    name: 'DigraphLineChart',
    props: {
        title: {
            type: String,
            default: 'EUR/USD Currency Rates'
        },
        data: {
            type: Array,
            required: true
        }
    },
    data(){
        return {
 	        
        };
    },
    watch: {
      data: {
        handler() {
          if(this.data) {
            const result = this.convertInputData();
            this.buildLineChart(result);
          }
        },
        deep: true,
        }
    },
    methods: {
        convertInputData() {
            return this.data
                .map( entry =>
                { 
                    const myMap = new Map().set('date', new Date(entry.date)).set('value1', entry.eurSaleRatePB).set('value2', entry.usdSaleRatePB);
                    const container = Array.from(myMap.values());
                    return container;
                }); 
        },
        buildLineChart(data){
            console.log(data);
            const lineChart = new Dygraph(
              'graphLineChart', 
              data,
              {
                labels: [ "date", "EUR", "USD"],
                fillGraph: false,
                strokeWidth: 3,
                series: {
                    'EUR':{
                      strokeWidth: 1.0,
                      drawPoints: true,
                      pointSize: 2.0,
                      highlightCircleSize: 5
                    },
                    'USD':{
                      strokeWidth: 1.5,
                      drawPoints: true,
                      pointSize: 2.5,
                      highlightCircleSize: 5.5
                    }
                }
              });
        }
    }
};
</script>

<style lang="scss" scoped>
#graphLineChart {
    margin-top: 20px;
    margin-left: 20px;
}
.legend {
    display: flex;
    justify-content: center;
    margin-top: 15px;
    margin-bottom: 45px;
    vertical-align: top;
}
.dygraph-axis-label-x,
.dygraph-axis-label-y {
    display: flex;
    justify-content: center;
}
</style>