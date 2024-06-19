function news(event) {
    event.preventDefault();

    const form = document.getElementById('news-form');
    const data = new FormData(form);
    const token = localStorage.getItem('accessToken');

    data.set('isPin', data.get('isPin') === 'true');

    fetch('http://localhost:5100/DermSight/News', {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${token}`
        },
        body: data
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json(); // 解析 JSON 格式的回應
    })
    .then(data => {
        console.log('API 回應:', data);
        if(data.status_code === 200){
            alert('新增成功！');
        }
        else{
            alert('新增失敗');
        }
        window.location.href = '../news/news.html';
    })
    .catch(error => {
        console.error('錯誤:', error);
    });
}
// 使用者登入註冊
function checkLoginStatus() {
    const userInfoContainer = document.getElementById('user-info');
    const token = localStorage.getItem('accessToken');

    if (token) {
        // 使用accessToken获取用户名等用户信息
        fetch("http://localhost:5100/DermSight/User/MySelf", {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
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
                alert("請先登入")
                window.location.href = '../../visitor/verify/verify.html';
            }
        })
        .catch(error => {
            console.error('Error fetching user info:', error);
            window.location.href = '../../visitor/index/index.html';
        });
    } else {
        alert("請先登入")
        window.location.href = '../../visitor/verify/verify.html';
    }
}

checkLoginStatus();