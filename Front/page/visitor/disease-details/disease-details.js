$(document).ready(function() {
    const urlParams = new URLSearchParams(window.location.search);
    const diseaseId = urlParams.get('diseaseId');

    $.ajax({
        url: `http://localhost:5100/DermSight/Disease?diseaseId=${diseaseId}`,
        method: 'GET',
        success: function(result) {
            console.log('Disease details:', result);
            renderDiseaseDetails(result);
        },
        error: function(error) {
            console.error('Error fetching disease details:', error);
            alert('獲取疾病詳情失敗，請稍後再試！');
        }
    });

    function renderDiseaseDetails(disease) {
        $('.disease-name').text(disease.name);
        $('.disease-description').text(disease.description);
    }
});



// 使用者登入註冊
$(document).ready(function() {
    function checkLoginStatus() {
        const userInfoContainer = $('#user-info');
        const accessToken = localStorage.getItem('accesstoken');

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


