// 最新消息
$(document).ready(function() {
    const searchInput = $('#searchInput');
    const searchBtn = $('#searchBtn');
    const diseaseContainer = $('#disease-container');
    const pagination = $('#pagination');

    // 初始化加载第一页的疾病列表
    fetchDiseases(1);

    // 搜索按钮点击事件
    searchBtn.on('click', function() {
        const searchTerm = searchInput.val().trim();
        if (searchTerm !== '') {
            fetchDiseases(1, searchTerm); // 搜索时默认加载第一页
        }
    });

    // 渲染疾病列表
    function renderDiseases(diseaseData) {
        diseaseContainer.empty(); // 清空之前的内容

        diseaseData.forEach(disease => {
            const diseaseElement = $(`
                <div class="disease">
                    <a href="#" class="disease-title" data-disease-id="${disease.diseaseId}">
                        ${disease.name}
                    </a>
                    <div class="disease-description">
                        <p>${disease.description}</p>
                    </div>
                </div>
            `);

            diseaseContainer.append(diseaseElement);
        });
    }

    // 获取疾病列表
    function fetchDiseases(page, search = '') {
        let url = `http://localhost:5100/DermSight/Disease/AllDisease?page=${page}`;
        if (search !== '') {
            url += `&Search=${search}`;
        }

        $.ajax({
            url: url,
            method: 'GET',
            success: function(result) {
                console.log('API Response:', result);
                if (result.status_code === 200 && result.data && Array.isArray(result.data.diseaseList)) {
                    renderDiseases(result.data.diseaseList);
                    renderPagination(result.data.forpaging.maxPage, page); // 更新分页导航
                } else {
                    console.error('Invalid data format:', result);
                    alert('獲取疾病列表失败，請稍後再試！');
                }
            },
            error: function(error) {
                console.error('Error fetching diseases:', error);
                alert('獲取疾病列表失败，請稍後再試！');
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
        fetchDiseases(page, searchInput.val().trim()); // 每次点击页码重新获取搜索框的值
    });

    // diseaseId裡面的資料
    diseaseContainer.on('click', '.disease-title', function(event) {
        event.preventDefault();
        const diseaseId = $(this).data('disease-id');
        console.log('Clicked diseaseId:', diseaseId);
        window.location.href = `../disease-details/disease-details.html?diseaseId=${diseaseId}`;
    });
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


