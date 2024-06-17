// 彈跳式視窗
document.addEventListener('DOMContentLoaded', (event) => {
    // 获取模态窗口
    const modal = document.getElementById('newsModal');
    const modalTitle = document.getElementById('modalTitle');
    const modalContent = document.getElementById('modalContent');

    // 获取打开模态窗口的按钮
    const btns = document.getElementsByClassName('openModalBtnNews');

    // 获取关闭按钮
    const closeButton = modal.getElementsByClassName('close')[0];

    // 当点击按钮时，打开模态窗口并更新内容
    for (let btn of btns) {
        btn.onclick = function(event) {
            event.preventDefault();  // 阻止默认行为（链接跳转）
            modalTitle.innerText = btn.getAttribute('data-title');
            modalContent.innerText = btn.getAttribute('data-content');
            modal.style.display = 'block';
        }
    }

    // 当点击关闭按钮时，关闭模态窗口
    closeButton.onclick = function() {
        modal.style.display = 'none';
    }

    // 当点击模态窗口外部时，关闭模态窗口
    window.onclick = function(event) {
        if (event.target === modal) {
            modal.style.display = 'none';
        }
    }
});

let currentPage = 1;

    function fetchNews(page) {
        $.ajax({
            url: `http://localhost:5100/DermSight/News/AllNews?page=${page}`,
            method: 'GET',
            success: function(result) {
                console.log('API Response:', result);
                if (result.status_code === 200 && result.data && Array.isArray(result.data.newsList)) {
                    renderNews(result.data.newsList);
                    renderPagination(result.data.forpaging.maxPage); // 假设返回的数据包含 maxPage 字段
                } else {
                    console.error('Invalid data format:', result);
                    alert('獲取新聞數據失敗，請稍後再試');
                }
            },
            error: function(error) {
                console.error('Error fetching news:', error);
                alert('發生錯誤，請稍後再試');
            }
        });
    }

    function renderNews(newsData) {
        const newsContainer = $('#news-container');
        newsContainer.empty(); // 清空之前的内容

        newsData.forEach(news => {
            const newsElement = $(`
                <li>
                    <a href="#" class="openModalBtnNews" data-title="${news.title}" data-content="${news.content}">
                        <div class="news-content">
                            <p class="new-type">${news.type}</p>
                            <p>${news.title}</p>
                        </div>
                        <p>${new Date(news.time).toLocaleDateString()}</p>
                    </a>
                </li>
            `);

            newsContainer.append(newsElement);
        });
    }

    function renderPagination(totalPages) {
        const paginationContainer = $('#pagination');
        paginationContainer.find('li:not(:first-child, :last-child)').remove(); // 清空之前的页码，但保留左右箭头

        for (let i = 1; i <= totalPages; i++) {
            const pageLink = $(`<li><a href="#" class="page-link" data-page="${i}">${i}</a></li>`);
            paginationContainer.find('li:last-child').before(pageLink);
        }

        $('.page-link').click(function(event) {
            event.preventDefault();
            const page = $(this).data('page');
            
            if (page === 'prev') {
                if (currentPage > 1) {
                    currentPage--;
                    fetchNews(currentPage);
                }
            } else if (page === 'next') {
                if (currentPage < totalPages) {
                    currentPage++;
                    fetchNews(currentPage);
                }
            } else {
                currentPage = page;
                fetchNews(currentPage);
            }
        });
    }

    // 初始加载第一页
    fetchNews(currentPage);