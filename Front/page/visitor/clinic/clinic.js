// 最新消息
$(document).ready(function() {
    const searchInput = $('#searchInput');
    const searchBtn = $('#searchBtn');
    const clinicContainer = $('#clinic-container');
    const pagination = $('#pagination');
    const cityContainer = $('#city');

    let cityId = 0;

    // 初始化加载第一页的疾病列表
    fetchclinics(1);
    fetchcitys();


    // 渲染疾病列表
    function renderclinics(clinicData) {
        clinicContainer.empty(); // 清空之前的内容

        clinicData.forEach(clinic => {
            const clinicElement = $(`
                <div class="clinic">
                    <a href="#" class="clinic-title" data-clinic-id="${clinic.clinicId}">
                        ${clinic.name}
                    </a>
                    <div class="clinic-description">
                        <p>地址<span>${clinic.address}</span></p>
                    </div>
                </div>
            `);

            clinicContainer.append(clinicElement);
        });
    }

    // 渲染診所列表
    function rendercity(cityData) {
        cityContainer.empty(); // 清空之前的内容

        cityData.forEach(city => {
            const cityElement = $(`
                <li>
                    <a href="#" class="city-id" data-city-id="${city.cityId}">
                        ${city.name}
                    </a>
                </li>
            `);

            cityContainer.append(cityElement);
        });
    }

    // 获取疾病列表
    function fetchclinics(page, cityId, search) {
        let url = `http://localhost:5100/DermSight/Clinic/AllClinic?page=${page}`;
        if (cityId) {
            url += `&cityId=${cityId}`;
        }
        
        if (search !== undefined && search.trim() !== '') {
            console.log(search)
            url += `&Search=${search}`;
        }

        $.ajax({
            url: url,
            method: 'GET',
            success: function(result) {
                console.log('API Response:', result);
                if (result.status_code === 200 && result.data && Array.isArray(result.data.clinicList)) {
                    renderclinics(result.data.clinicList);
                    renderPagination(result.data.forpaging.maxPage, page); // 更新分页导航
                } else {
                    console.error('Invalid data format:', result);
                    alert('獲取診所列表失敗，請稍後再試！');
                }
            },
            error: function(error) {
                console.error('Error fetching clinics:', error);
                alert('獲取診所列表失敗，請稍後再試！');
            }
        });
    }

    // 获取疾病列表
    function fetchcitys() {
        let url = `http://localhost:5100/DermSight/Clinic/AllCity`;

        $.ajax({
            url: url,
            method: 'GET',
            success: function(result) {
                console.log('API Response:', result);
                if (result.status_code === 200 && result.data && Array.isArray(result.data)) {
                    rendercity(result.data);
                } else {
                    console.error('Invalid data format:', result);
                    alert('獲取縣市列表失敗，請稍後再試！');
                }
            },
            error: function(error) {
                console.error('Error fetching clinics:', error);
                alert('獲取縣市列表失敗，請稍後再試！');
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

    // 搜索按钮点击事件
    searchBtn.on('click', function(event) {
        fetchclinics(1, cityId, searchInput.val().trim());
    });
    // 点击分页按钮事件
    pagination.on('click', '.page-link', function(event) {
        event.preventDefault();
        const page = $(this).data('page');
        fetchclinics(page, cityId, searchInput.val().trim());
    });

    // clinicId裡面的資料
    clinicContainer.on('click', '.clinic-title', function(event) {
        event.preventDefault();
        const clinicId = $(this).data('clinic-id');
        console.log('Clicked clinicId:', clinicId);
        window.location.href = `clinic-details.html?clinicId=${clinicId}`;
    });

    cityContainer.on('click', '.city-id', function(event) {
        event.preventDefault();
        cityId = $(this).data('city-id');
        console.log('Clicked cityId:', cityId);
        fetchclinics(1, cityId, searchInput.val().trim());
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


