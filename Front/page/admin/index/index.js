$(document).ready(function() {
    // 定义全局变量用于存储当前页码
    let currentNewsPage = 1;
    let currentDiseasePage = 1;

    // #region 最新消息
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
        $('#news-modal-type').text(type);
        $('#news-modal-title').text(title);
        $('#news-modal-content').text(content);
        $('#news-modal').css('display', 'block');
    }

    function closeModal() {
        $('#news-modal').css('display', 'none');
        $('#disease-modal').css('display', 'none');
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
        if ($(event.target).is('#disease-modal')) {
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
                    renderPagination(result.data.forpaging.maxPage, page, 'news');
                    currentNewsPage = page; // 更新当前页码
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

    // #endregion

    // #region 皮膚資訊
    function renderDisease(diseaseData) {
        const diseaseContainer = $('#disease-container ul');
        diseaseContainer.empty();

        diseaseData.forEach(disease => {
            const diseaseElement = $(`
                <li>
                    <a href="#" class="openModalBtnDisease" data-title="${disease.name}" data-content="${disease.description}">
                        <div class="disease-content">
                            <p class="new-type">病徵</p>
                            <p>${disease.name}</p>
                        </div>
                        <p>${new Date(disease.time).toLocaleDateString()}</p>
                    </a>
                </li>
            `);

            diseaseContainer.append(diseaseElement);
        });
    }

    function showModalDisease(name, description) {
        $('#disease-modal-title').text(name);
        $('#disease-modal-content').text(description);
        $('#disease-modal').css('display', 'block');
    }

    $('#disease-container').on('click', '.openModalBtnDisease', function(event) {
        event.preventDefault();
        const name = $(this).data('name');
        const description = $(this).data('description');
        showModalDisease(name, description);
    });

    function fetchDisease(page) {
        $.ajax({
            url: `http://localhost:5100/DermSight/Disease/AllDisease?page=${page}`,
            method: 'GET',
            success: function(result) {
                console.log('API Response:', result);
                if (result.status_code === 200 && result.data && Array.isArray(result.data.diseaseList)) {
                    renderDisease(result.data.diseaseList);
                    renderPagination(result.data.forpaging.maxPage, page, 'disease');
                    currentDiseasePage = page; // 更新当前页码
                } else {
                    console.error('Invalid data format:', result);
                    alert('獲取相關資訊失敗，請稍後再試');
                }
            },
            error: function(error) {
                console.error('Error fetching diseases:', error);
                alert('獲取疾病列表失败，請稍後再試！');
            }
        });
    }

    // #endregion

    function renderPagination(maxPage, currentPage, type) {
        const paginationContainer = $(`#${type}-pagination`);
        paginationContainer.empty(); // 清空之前的分页

        // 创建分页按钮
        for (let i = 1; i <= maxPage; i++) {
            const pageItem = $(`
                <li>
                    <a href="#" class="page-link" data-page="${i}" data-type="${type}">${i}</a>
                </li>
            `);

            if (i === currentPage) {
                pageItem.find('a').css('font-weight', 'bold'); // 当前页高亮显示
            }

            paginationContainer.append(pageItem);
        }
    }

    // 监听分页按钮点击事件
    $(document).on('click', '.page-link', function(event) {
        event.preventDefault();
        const page = $(this).data('page');
        const type = $(this).data('type');

        if (type === 'news') {
            fetchNews(page);
        } else if (type === 'disease') {
            fetchDisease(page);
        }
    });

    // 初始加载第一页新闻和疾病数据
    fetchNews(1);
    fetchDisease(1);


    // 使用者登入註冊
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
                    alert("請先登入")
                    window.location.href = '../../visitor/verify/verify.html';
                }
            })
            .catch(error => {
                console.error('Error fetching user info:', error);
                window.location.href = '../../visitor/index/index.html';
            });
        } else {
            alert("請先登入 沒token")
            window.location.href = '../../visitor/verify/verify.html';
        }
    }

    checkLoginStatus();
});

