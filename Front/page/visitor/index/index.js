// 最新消息
$(document).ready(function() {
    function renderNews(newsData) {
        const newsContainer = $('#news-container ul');
        newsContainer.empty(); // 清空之前的内容

        newsData.forEach(news => {
            const newsElement = $(`
                <li>
                    <a href="#" class="openModalBtnNews" data-type="${news.type}" data-title="${news.title}" data-content="${news.content}">
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

    function showModal(type, title, content) {
        $('#modal-type').text(type);
        $('#modal-title').text(title);
        $('#modal-content').text(content);
        $('#news-modal').css('display', 'block');
    }

    function closeModal() {
        $('#news-modal').css('display', 'none');
    }

    $('#news-container').on('click', '.openModalBtnNews', function(event) {
        event.preventDefault();
        const type = $(this).data('type');
        const title = $(this).data('title');
        const content = $(this).data('content');
        showModal(type, title, content);
    });

    $('.close').on('click', function() {
        closeModal();
    });

    $(window).on('click', function(event) {
        if ($(event.target).is('#news-modal')) {
            closeModal();
        }
    });

    function fetchNews(page) {
        $.ajax({
            url: `http://localhost:5100/DermSight/News/AllNews?page=${page}`,
            method: 'GET',
            success: function(result) {
                console.log('API Response:', result);
                if (result.status_code === 200 && result.data && Array.isArray(result.data.newsList)) {
                    renderNews(result.data.newsList);
                    renderPagination(result.data.forpaging.maxPage, page);
                } else {
                    console.error('Invalid data format:', result);
                    alert('獲取相關資訊失敗，請稍後再試');
                }
            },
            error: function(error) {
                console.error('Error fetching news:', error);
                alert('發生錯誤，請稍後再試');
            }
        });
    }

    function renderPagination(maxPage, currentPage) {
        const paginationContainer = $('#pagination');
        paginationContainer.empty(); // 清空之前的分页

        // 创建分页按钮
        for (let i = 1; i <= maxPage; i++) {
            const pageItem = $(`
                <li>
                    <a href="#" class="page-link" data-page="${i}">${i}</a>
                </li>
            `);

            if (i === currentPage) {
                pageItem.find('a').css('font-weight', 'bold'); // 当前页高亮显示
            }

            paginationContainer.append(pageItem);
        }
    }

    // 监听分页按钮点击事件
    $('#pagination').on('click', '.page-link', function(event) {
        event.preventDefault();
        const page = $(this).data('page');
        fetchNews(page);
    });

    // 初始加载第一页新闻
    fetchNews(1);
});

// 使用者登入註冊
$(document).ready(function() {
    function checkLoginStatus() {
        const userInfoContainer = $('#user-info');
        const accessToken = localStorage.getItem('accessToken');

        if (accessToken) {
            // 使用accessToken获取用户名等用户信息
            fetch("http://localhost:5100/DermSight/User/MySelf", {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${accessToken}`
                }
            })
            .then(response => response.json())
            .then(result => {
                if (result.status_code === 200) {
                    const name = result.data.name; // 假设返回的数据包含用户名
                    userInfoContainer.html(`
                        <a href="../user/user.html">
                            <i class="fa-solid fa-circle-user"></i>
                            ${name}
                        </a>
                    `);
                } else {
                    // 令牌无效或其他错误，显示登录/注册链接
                    userInfoContainer.html(`
                        <a href="../verify/verify.html">登入 / 註冊</a>
                    `);
                }
            })
            .catch(error => {
                console.error('Error fetching user info:', error);
                userInfoContainer.html(`
                    <a href="../verify/verify.html">登入 / 註冊</a>
                `);
            });
        } else {
            // 用户未登录
            userInfoContainer.html(`
                <a href="../verify/verify.html">登入 / 註冊</a>
            `);
        }
    }

    checkLoginStatus();
});


