// document.addEventListener('DOMContentLoaded', (event) => {
//     // 获取模态窗口
//     const modal = document.getElementById('newsModal');
//     const modalTitle = document.getElementById('modalTitle');
//     const modalContent = document.getElementById('modalContent');

//     // 获取打开模态窗口的按钮
//     const btns = document.getElementsByClassName('openModalBtnNews');

//     // 获取关闭按钮
//     const closeButton = modal.getElementsByClassName('close')[0];

//     // 当点击按钮时，打开模态窗口并更新内容
//     for (let btn of btns) {
//         btn.onclick = function(event) {
//             event.preventDefault();  // 阻止默认行为（链接跳转）
//             modalTitle.innerText = btn.getAttribute('data-title');
//             modalContent.innerText = btn.getAttribute('data-content');
//             modal.style.display = 'block';
//         }
//     }

//     // 当点击关闭按钮时，关闭模态窗口
//     closeButton.onclick = function() {
//         modal.style.display = 'none';
//     }

//     // 当点击模态窗口外部时，关闭模态窗口
//     window.onclick = function(event) {
//         if (event.target === modal) {
//             modal.style.display = 'none';
//         }
//     }
// });


// 最新消息
$(document).ready(function() {
    const searchInput = $('#searchInput');
    const searchBtn = $('#searchBtn');
    const clinicContainer = $('#clinic-container');
    const pagination = $('#pagination');

    // 初始化加载第一页的疾病列表
    fetchclinic(1);

    // 搜索按钮点击事件
    searchBtn.on('click', function() {
        const searchTerm = searchInput.val().trim();
        if (searchTerm !== '') {
            fetchclinic(1, searchTerm); // 搜索时默认加载第一页
        }
    });

    // 渲染疾病列表
    function renderclinic(clinicData) {
        clinicContainer.empty(); // 清空之前的内容

        clinicData.forEach(clinic => {
            const clinicElement = $(`
                <tr>
                    <td>
                        ${clinic.name}
                    </td>
                    <td>
                        ${clinic.phone}
                    </td>
                    <td>
                        ${clinic.address}
                    </td>
                    <td>
                        <a href="#">修改</a>
                        <a href="#">刪除</a>
                    </td>
                </tr>
            `);

            clinicContainer.append(clinicElement);
        });
    }

    // 获取疾病列表
    function fetchclinic(page, search = '') {
        let url = `http://localhost:5100/DermSight/clinic/Allclinic?page=${page}`;
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
                if (result.status_code === 200 && result.data && Array.isArray(result.data.clinicList)) {
                    renderclinic(result.data.clinicList);
                    renderPagination(result.data.forpaging.maxPage, page); // 更新分页导航
                } else {
                    console.error('Invalid data format:', result);
                    alert('獲取最新消息列表失败，請稍後再試！');
                }
            },
            error: function(error) {
                console.error('Error fetching clinic:', error);
                alert('獲取最新消息列表失败，請稍後再試！');
            }
        });
    }

    // 渲染分页导航
    function renderPagination(maxPage, currentPage) {
        if(currentPage<=maxPage){
            pagination.empty(); // 清空之前的分页
            pagination.append($(`
                <li>
                    <a href="#" class="page-link" data-page="${1}"><<</a>
                </li>
                <li>
                    <a href="#" class="page-link" data-page="${currentPage - 1}"><</a>
                </li>
            `));
            if(currentPage<4){
                // 创建分页按钮
                for (let i = currentPage < 4 ? 1 : currentPage - 2 ; i <= 5 ; i++) {
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
            else{
                // 创建分页按钮
                for (let i = currentPage < 4 ? 1 : currentPage - 2 ; i <= (currentPage + 2 > maxPage ? maxPage : currentPage + 2) ; i++) {
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
            pagination.append($(`
                <li>
                    <a href="#" class="page-link" data-page="${currentPage + 1}">></a>
                </li>
                <li>
                    <a href="#" class="page-link" data-page="${maxPage}">>></a>
                </li>
            `));
        }
    }

    // 点击分页按钮事件
    pagination.on('click', '.page-link', function(event) {
        event.preventDefault();
        const page = $(this).data('page');
        fetchclinic(page, searchInput.val().trim()); // 每次点击页码重新获取搜索框的值
    });

    // clinicId裡面的資料
    clinicContainer.on('click', '.clinic-title', function(event) {
        event.preventDefault();
        const clinicId = $(this).data('clinic-id');
        console.log('Clicked clinicId:', clinicId);
        window.location.href = `clinic-details.html?clinicId=${clinicId}`;
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