$(document).ready(function() {
    function disease(event) {
        event.preventDefault();

        const form = document.getElementById('disease-form');
        const data = new FormData(form);
        const token = localStorage.getItem('accessToken');

        const isPinValue = data.get('isPin') === 'true' ? 'true' : 'false';
            data.set('isPin', isPinValue);

        fetch('http://localhost:5100/DermSight/disease', {
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
            window.location.href = '../disease/disease.html';
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
    $('#disease-form').on('submit', function(event) {
        disease(event);
    });
    
    checkLoginStatus();
    $('#disease-form').on('submit', function(event) {
        disease(event);
    });
    
    $('#edit-disease-form').on('submit', function(event) {
        
        const urlParams = new URLSearchParams(window.location.search);
        const diseaseId = urlParams.get('id');
        
        event.preventDefault();
        const formData = new FormData(this);

        if(diseaseId !== null){
            formData.set('diseaseId', diseaseId);    
        
            fetch(`http://localhost:5100/DermSight/disease`, {
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
                    window.location.href = '../disease/disease.html'; // Redirect back to disease list
                } else {
                    alert('修改失敗！');
                }
            })
            .catch(error => {
                console.error('Error:', error);
                alert('Error updating disease');
            });
        }
        // 沒有抓到Id則新增
        else{
            // formData.set("symptoms",formData.get("symptoms").split(','))
            fetch(`http://localhost:5100/DermSight/disease`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
                },
                body: formData
            })
            .then(response => response.json())
            .then(data => {
                if (data.status_code === 200) {
                    alert('新增成功！');
                    window.location.href = '../disease/disease.html'; // Redirect back to disease list
                } else {
                    alert('新增失敗！');
                }
            })
            .catch(error => {
                console.error('Error:', error);
                alert('Error updating disease');
            });
        }
    });
});

$(document).ready(function() {
    const urlParams = new URLSearchParams(window.location.search);
    const diseaseId = urlParams.get('id');

    // Fetch disease data based on ID
    // 有抓到id則帶入資料修改
    if (diseaseId !== null) {
        fetch(`http://localhost:5100/DermSight/disease?diseaseId=${diseaseId}`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
            }
        })
        .then(response => response.json())
        .then(data => {
            if (data.status_code === 200) {
                const disease = data.data.disease;
                const symptoms = data.data.symptoms;
                var symptomsdata = ""
                $('#name').val(disease.name);
                $('#description').val(disease.description);
                symptoms.forEach((data,index) => {
                    if(index === symptoms.length - 1){
                        symptomsdata += data.content; // 不加逗號
                    }
                    else {
                        symptomsdata += data.content + ","; // 加逗號
                    }
                });
                $('#symptoms').val(symptomsdata);
            } else {
                alert('Error fetching disease data');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Failed to fetch disease data');
        });
    }
});