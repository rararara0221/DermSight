// 最新消息
$(document).ready(function() {
    const searchInput = $('#searchInput');
    const searchBtn = $('#searchBtn');
    const newsContainer = $('#news-container');
    const pagination = $('#pagination');

    // 初始化加载第一页的疾病列表
    fetchnews(1);

    // 搜索按钮点击事件
    searchBtn.on('click', function() {
        const searchTerm = searchInput.val().trim();
        if (searchTerm !== '') {
            fetchnews(1, searchTerm); // 搜索时默认加载第一页
        }
    });

    // 渲染疾病列表
    function rendernews(newsData) {
        newsContainer.empty(); // 清空之前的内容

        newsData.forEach(news => {
            const newsElement = $(`
                <tr>
                    <td>
                        ${news.type}
                    </td>
                    <td>
                        ${news.title}
                    </td>
                    <td>
                        ${news.content}
                    </td>
                    <td>
                        <a href="../news-data/news-data.html?id=${news.newsId}">修改</a>
                        <a href="#" onclick="deleteNews(${news.newsId})">刪除</a>
                    </td>
                </tr>
            `);

            newsContainer.append(newsElement);
        });
    }

    // 获取疾病列表
    function fetchnews(page, search = '') {
        let url = `http://localhost:5100/DermSight/News/AllNews?page=${page}`;
        if (search !== '') {
            url += `&Search=${search}`;
        }
        const accessToken = localStorage.getItem('accessToken');

        $.ajax({
            url: url,
            method: 'GET',
            headers: {
                        "Authorization": "Bearer " + accessToken
                    },
            success: function(result) {
                console.log('API Response:', result);
                if (result.status_code === 200 && result.data && Array.isArray(result.data.newsList)) {
                    rendernews(result.data.newsList);
                    renderPagination(result.data.forpaging.maxPage, page); // 更新分页导航
                } else {
                    console.error('Invalid data format:', result);
                    alert('獲取最新消息列表失败，請稍後再試！');
                }
            },
            error: function(error) {
                console.error('Error fetching news:', error);
                alert('獲取最新消息列表失败，請稍後再試！');
            }
        });
    }

    // 渲染分页导航
    function renderPagination(maxPage, currentPage) {
        pagination.empty(); // 清空之前的分页

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

            pagination.append(pageItem);
        }
    }

    // 点击分页按钮事件
    pagination.on('click', '.page-link', function(event) {
        event.preventDefault();
        const page = $(this).data('page');
        fetchnews(page, searchInput.val().trim()); // 每次点击页码重新获取搜索框的值
    });

    // newsId裡面的資料
    newsContainer.on('click', '.news-title', function(event) {
        event.preventDefault();
        const newsId = $(this).data('news-id');
        console.log('Clicked newsId:', newsId);
        window.location.href = `news-details.html?newsId=${newsId}`;
    });
});

function deleteNews(newsId) {
    if (confirm('確定要刪除這條最新消息嗎？')) {
        fetch(`http://localhost:5100/DermSight/News?NewsId=${newsId}`, {
            method: 'DELETE',
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
            }
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            if (data.status_code === 200) {
                alert('刪除成功');
                location.reload(); // Reload the page to reflect changes
            } else {
                alert('刪除失敗');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('刪除失敗');
        });
    }
}




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


