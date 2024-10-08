﻿function createChart(divId, label, chartData, chartType = 'line', timeUnit = 'hour', borderColor = 'rgba(75, 192, 192, 1)', backgroundColor = 'rgb(0, 219, 155)') {
    const labels = Object.keys(chartData);  // Tarih etiketleri
    const data = Object.values(chartData);  // İzlenme sayıları

    const ctx = document.getElementById(divId).getContext('2d');

    // Mevcut grafik varsa önce yok et (Yeniden kullanılabilirlik için)
    if (window.myChartInstance) {
        window.myChartInstance.destroy();
    }

    window.myChartInstance = new Chart(ctx, {
        type: chartType,  // Grafik tipi ('line', 'bar', vs.)
        data: {
            labels: labels,  // X ekseni: Zaman dilimleri
            datasets: [{
                label: label,  // Grafiğin etiketi
                data: data,  // Y ekseni: İzlenme sayıları
                borderColor: borderColor,  // Kenar rengi
                backgroundColor: backgroundColor,  // Arka plan rengi
                fill: true,
                tension: 0.3,  // Çizginin eğriliği (yumuşatma)
                spanGaps: true  // Boşluklar arasında çizgi çiz
            }]
        },
        options: {
            responsive: true,  // Sayfa boyutuna göre otomatik boyutlandırma
            maintainAspectRatio: false,  // En-boy oranını korumamak için
            scales: {
                x: {
                    type: 'time',  // X ekseni zaman ölçeği
                    time: {
                        unit: timeUnit,  // Dinamik zaman birimi ('hour' veya 'day')
                        displayFormats: {
                            hour: 'MMM D, HH:mm',  // Saat formatı
                            day: 'MMM D'  // Gün formatı
                        },
                        tooltipFormat: 'MMM D, HH:mm'  // Tooltip formatı
                    },
                    ticks: {
                        autoSkip: true,  // Fazla olan etiketleri atla
                        maxTicksLimit: 10  // Maksimum etiket sayısını sınırla
                    }
                },
                y: {
                    beginAtZero: true  // Y ekseni sıfırdan başlasın
                }
            },
            plugins: {
                legend: {
                    display: true,  // Etiket göster
                    position: 'top'  // Etiket pozisyonu
                }
            }
        }
    });
}
