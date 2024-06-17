function showPage(pageId) {
    // 隱藏所有頁面
    var pages = document.querySelectorAll('.show');
    pages.forEach(function(page) {
        page.classList.remove('active');
    });

    // 顯示選中的頁面
    var activePage = document.getElementById(pageId);
    activePage.classList.add('active');
}

var ctx = document.getElementById('accuracyChart').getContext('2d');
        var accuracyChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: ['1月', '2月', '3月', '4月', '5月', '6月'],
                datasets: [{
                    label: '準確率',
                    data: [90, 92, 94, 95, 94, 95],
                    borderColor: 'blue',
                    fill: false
                }, {
                    label: '召回率',
                    data: [88, 90, 91, 93, 92, 93],
                    borderColor: 'green',
                    fill: false
                }, {
                    label: 'F1 分數',
                    data: [89, 91, 92, 94, 93, 94],
                    borderColor: 'red',
                    fill: false
                }]
            },
            options: {
                responsive: true,
                scales: {
                    x: {
                        title: {
                            display: true,
                            text: '月份'
                        }
                    },
                    y: {
                        title: {
                            display: true,
                            text: '百分比'
                        },
                        min: 0,
                        max: 100
                    }
                }
            }
        });