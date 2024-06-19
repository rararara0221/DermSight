// 使用者
$(document).ready(function () {
    const userInfoContainer = $("#user-info");
    const userContainer = $("#user-container");

    // 获取用户信息
    function getUserInfo() {
        const accessToken = localStorage.getItem("accesstoken");

        if (!accessToken) {
            userInfoContainer.html(`<a href="../verify/verify.html">
                                        登入 / 註冊
                                    </a>`);
            return;
        }

        $.ajax({
            url: "http://localhost:5100/DermSight/User/MySelf",
            method: "GET",
            headers: {
                Authorization: `Bearer ${accessToken}`,
            },
            success: function (result) {
                if (result.status_code === 200) {
                    const { name, account, mail, photo } = result.data;

                    userInfoContainer.html(`
                        <a href="../user/user.html">
                            <i class="fa-solid fa-circle-user"></i>
                            ${name}
                        </a>
                    `);

                    userContainer.html(`
                        <div class="user-picture">
                            <img src="${photo}">
                        </div>
                        <div class="user-title">
                            ${name}
                        </div>
                        <div class="user-data">
                            <p>${account}</p>
                            <p>${mail}</p>
                        </div>
                        <div class="user-btn">
                            <a href="#" class="user-update" id="openModalBtnEdit">
                                編輯
                            </a>
                            <a href="#" class="user-update" id="openModalBtnPwd">
                                修改密碼
                            </a>
                            <a href="#" class="user-logout" id="logout-btn">
                                登出
                            </a>
                        </div>
                    `);

                    // 绑定事件处理程序
                    bindEventHandlers();
                } else {
                    userInfoContainer.html(`<a href="../verify/verify.html">登入 / 註冊</a>`);
                }
            },
            error: function (error) {
                console.error("Error fetching user info:", error);
                userInfoContainer.html(`<a href="../verify/verify.html">登入 / 註冊</a>`);
            },
        });
    }

    function bindEventHandlers() {
        const modalEdit = $("#modalEdit");
        const modalPwd = $("#modalPwd");

        $("#openModalBtnEdit").on("click", function (event) {
            event.preventDefault();
            modalEdit.show();
        });

        $("#openModalBtnPwd").on("click", function (event) {
            event.preventDefault();
            modalPwd.show();
        });

        $(".close").on("click", function () {
            modalEdit.hide();
            modalPwd.hide();
        });

        $(window).on("click", function (event) {
            if ($(event.target).is(modalEdit)) {
                modalEdit.hide();
            }
            if ($(event.target).is(modalPwd)) {
                modalPwd.hide();
            }
        });

        $("#logout-btn").on("click", function () {
            localStorage.removeItem("accesstoken");
            location.reload();
        });

        $("#edit-form").on("submit", function (event) {
            event.preventDefault();
            const formData = $(this).serialize();
            const accessToken = localStorage.getItem("accesstoken");

            $.ajax({
                url: "http://localhost:5100/DermSight/User/Edit",
                method: "POST",
                headers: {
                    Authorization: `Bearer ${accessToken}`,
                },
                data: formData,
                success: function (response) {
                    alert("信息编辑成功");
                    modalEdit.hide();
                    getUserInfo(); // 更新用户信息
                },
                error: function (error) {
                    console.error("Error editing user info:", error);
                    alert("编辑信息失败");
                },
            });
        });

        $("#pwd-form").on("submit", function (event) {
            event.preventDefault();
            const formData = $(this).serialize();
            const accessToken = localStorage.getItem("accesstoken");

            $.ajax({
                url: "http://localhost:5100/DermSight/User/ChangePassword",
                method: "POST",
                headers: {
                    Authorization: `Bearer ${accessToken}`,
                },
                data: formData,
                success: function (response) {
                    alert("密码修改成功");
                    modalPwd.hide();
                },
                error: function (error) {
                    console.error("Error changing password:", error);
                    alert("修改密码失败");
                },
            });
        });
    }

    // 初始化
    getUserInfo();
});

// 最新消息
$(document).ready(function() {
    function renderrecord(recordData) {
        const recordContainer = $('#record-container');
        recordContainer.empty(); // 清空之前的内容

        recordData.forEach(record => {
            const recordElement = $(`

                <li class="record-disease">
                        <img src="../../../img/disease.jpg">
                        <div class="record-content">
                            <p class="record-diseasename">
                                接觸性皮膚炎
                            </p>
                            <p class="record-date">
                                ${new Date(record.time).toLocaleDateString()}
                            </p>
                        </div>
                    </li>

                <li>
                    <a href="#" class="openModalBtnrecord" data-type="${record.type}" data-title="${record.title}" data-content="${record.content}">
                        <div class="record-content">
                            <p class="new-type">${record.type}</p>
                            <p>${record.title}</p>
                        </div>
                        <p>${new Date(record.time).toLocaleDateString()}</p>
                    </a>
                </li>
            `);

            recordContainer.append(recordElement);
        });
    }

    function showModal(type, title, content) {
        $('#modal-type').text(type);
        $('#modal-title').text(title);
        $('#modal-content').text(content);
        $('#record-modal').css('display', 'block');
    }

    function closeModal() {
        $('#record-modal').css('display', 'none');
    }

    $('#record-container').on('click', '.openModalBtnrecord', function(event) {
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
        if ($(event.target).is('#record-modal')) {
            closeModal();
        }
    });

    function fetchrecord(page) {
        $.ajax({
            url: `http://localhost:5100/DermSight/record/Allrecord?page=${page}`,
            method: 'GET',
            success: function(result) {
                console.log('API Response:', result);
                if (result.status_code === 200 && result.data && Array.isArray(result.data.recordList)) {
                    renderrecord(result.data.recordList);
                    renderPagination(result.data.forpaging.maxPage, page);
                } else {
                    console.error('Invalid data format:', result);
                    alert('獲取相關資訊失敗，請稍後再試');
                }
            },
            error: function(error) {
                console.error('Error fetching record:', error);
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
        fetchrecord(page);
    });

    // 初始加载第一页新闻
    fetchrecord(1);
});