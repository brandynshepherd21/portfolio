
var data = {
    labels: ["January", "February", "March", "April", "May", "June", "July"],
    datasets: [
        {
            label: "Gross Profits",
            fillColor: "rgba(63, 127, 191,0.2)",
            strokeColor: "rgba(63, 127, 191,1)",
            pointColor: "rgba(63, 127, 191,1)",
            pointStrokeColor: "#fff",
            pointHighlightFill: "#fff",
            pointHighlightStroke: "rgba(63, 127, 191,1)",
            data: [65, 60, 80, 85, 55, 60, 40]
        },
        {
            label: "Amazon Contribution",
            fillColor: "rgba(219, 4, 4,0.2)",
            strokeColor: "rgba(219, 4, 4,1)",
            pointColor: "rgba(219, 4, 4,1)",
            pointStrokeColor: "#fff",
            pointHighlightFill: "#fff",
            pointHighlightStroke: "rgba(219, 4, 4,1)",
            data: [13, 12, 16, 17, 11, 12, 8]
        },
        {
            label: "Net Profits",
            fillColor: "rgba(4, 219, 4,0.2)",
            strokeColor: "rgba(4, 219, 4,1)",
            pointColor: "rgba(4, 219, 4,1)",
            pointStrokeColor: "#fff",
            pointHighlightFill: "#fff",
            pointHighlightStroke: "rgba(4, 219, 4,1)",
            data: [52, 48, 64, 68, 44, 48, 32]
        }
    ]
};
var ctx = document.getElementById("myChart").getContext("2d");
new Chart(ctx).Line(data, {responsive: true});