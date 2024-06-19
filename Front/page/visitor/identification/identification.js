$(document).ready(function() {
    // 使用者登入註冊
    function checkLoginStatus() {
        const userInfoContainer = $('#user-info');
        const accessToken = localStorage.getItem('accesstoken');

        if (accessToken) {
            fetch("http://localhost:5100/DermSight/User/MySelf", {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${accessToken}`
                }
            })
            .then(response => response.json())
            .then(result => {
                if (result.status_code === 200) {
                    const name = result.data.name;
                    userInfoContainer.html(`
                        <a href="../user/user.html">
                            <i class="fa-solid fa-circle-user"></i>
                            ${name}
                        </a>
                    `);
                } else {
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
            userInfoContainer.html(`
                <a href="../verify/verify.html">登入 / 註冊</a>
            `);
        }
    }

    checkLoginStatus();

    // 預覽圖片
    document.getElementById('imageUpload').addEventListener('change', function(event) {
        const file = event.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function(e) {
                const previewImage = document.getElementById('imagePreview');
                previewImage.src = e.target.result;
                previewImage.style.display = 'block';
            };
            reader.readAsDataURL(file);
        }
    });

    // 上傳檔案
    document.getElementById('upload').addEventListener('click', function() {
        var fileInput = document.getElementById('imageUpload');
        const accessToken = localStorage.getItem('accesstoken');
        var file = fileInput.files[0];
    
        if (!file) {
            alert('請選擇一個檔案');
            return;
        }
    
        var formData = new FormData();
        formData.append('Photo', file);
    
        fetch('http://localhost:5100/DermSight/Identification', {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${accessToken}`
            },
            body: formData
        })
        .then(response => {
            console.log("API 回傳的資料:", response);
    
            if (response.status === 400) {
                alert("請先登入");
                window.location.href = '../verify/verify.html';
            }
    
            return response.json();
        })
        .then(result => {
            alert("上傳成功！");
            document.getElementById('result').style.display = 'block';
        })
        .catch(error => {
            console.error('發生錯誤:', error);
            alert('發生錯誤，請稍後再試');
        });
    });
});