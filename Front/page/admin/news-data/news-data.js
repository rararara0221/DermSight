$(document).ready(function() {
    function news(event) {
        event.preventDefault();

        const form = document.getElementById('edit-news-form');
        const data = new FormData(form);
        const token = localStorage.getItem('accessToken');

        const isPinValue = data.get('isPin') === 'true' ? true : false;
            data.set('isPin', isPinValue);

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
                    // window.location.href = '../../visitor/verify/verify.html';
                }
            })
            .catch(error => {
                console.error('Error fetching user info:', error);
                // window.location.href = '../../visitor/index/index.html';
            });
        } else {
            alert("請先登入")
            // window.location.href = '../../visitor/verify/verify.html';
        }
    }

    checkLoginStatus();
    $('#news-form').on('submit', function(event) {
        news(event);
    });
    
    $('#edit-news-form').on('submit', function(event) {
        
        const urlParams = new URLSearchParams(window.location.search);
        const newsId = urlParams.get('id');
        if(newsId===null){
            news(event);
        }
        else{
            event.preventDefault();
        
            const formData = new FormData(this);
        
            const isPinValue = formData.get('isPin') === 'true' ? 'true' : 'false';
            formData.set('isPin', isPinValue);
            formData.set('newsId', newsId);    
        
            fetch(`http://localhost:5100/DermSight/News`, {
                method: 'PUT',
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
                },
                body: formData
            })
            .then(response => response.json())
            .then(data => {
                if (data.status_code === 200) {
                    alert('修改成功！');
                    window.location.href = '../news/news.html'; // Redirect back to news list
                } else {
                    alert('修改失敗！');
                }
            })
            .catch(error => {
                console.error('Error:', error);
                alert('Error updating news');
            });
        }
    });
});

$(document).ready(function() {
    const urlParams = new URLSearchParams(window.location.search);
    const newsId = urlParams.get('id');

    // Fetch news data based on ID
    if (newsId) {
        fetch(`http://localhost:5100/DermSight/News?NewsId=${newsId}`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
            }
        })
        .then(response => response.json())
        .then(data => {
            if (data.status_code === 200) {
                const news = data.data;
                $('#title').val(news.title);
                $('#type').val(news.type);
                $('#content').val(news.content);
                $('#isPin').val(news.isPin.toString()); // Convert boolean to string
            } else {
                alert('Error fetching news data');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Failed to fetch news data');
        });
    }
});
