// 登入註冊顯示
function showLogin() {
    document.getElementById('login-form').style.display = 'block';
    document.getElementById('forget-form').style.display = 'none';
    document.getElementById('register-form').style.display = 'none';
    document.getElementById('login-btn').classList.add('active');
    document.getElementById('register-btn').classList.remove('active');
    document.getElementById('forget-btn').classList.remove('active');
}

function showRegister() {
    document.getElementById('login-form').style.display = 'none';
    document.getElementById('register-form').style.display = 'block';
    document.getElementById('forget-form').style.display = 'none';
    document.getElementById('register-btn').classList.add('active');
    document.getElementById('login-btn').classList.remove('active');
    document.getElementById('forget-btn').classList.remove('active');
}

function showForget() {
    document.getElementById('login-form').style.display = 'none';
    document.getElementById('register-form').style.display = 'none';
    document.getElementById('forget-form').style.display = 'block';
    document.getElementById('login-btn').classList.remove('active');
    document.getElementById('register-btn').classList.remove('active');
}

function showForgetAuthcode() {
    document.getElementById('forget-form').style.display = 'none';
    document.getElementById('forget-authcode-form').style.display = 'block';
}

function showForgetChangepassword() {
    document.getElementById('forget-authcode-form').style.display = 'none';
    document.getElementById('forget-changepassword-form').style.display = 'block';
}

showLogin();

// 註冊
async function register(event) {
    event.preventDefault();

    const form = document.getElementById('register-form');
    const formData = new FormData(form);

    // 简单的验证逻辑
    const data = {
        Name: formData.get('Name'),
        Account: formData.get('Account'),
        Password: formData.get('Password'),
        PasswordCheck: formData.get('PasswordCheck'),
        Mail: formData.get('Mail')
    };

    if (!data.Name || !data.Account || !data.Password || !data.PasswordCheck || !data.Mail) {
        alert("請確切輸入註冊資料");
        return;
    }

    if (data.Account.length < 6 || data.Account.length > 20) {
        alert('帳號長度需介於6-20字元');
        return;
    }

    if (data.Password !== data.PasswordCheck) {
        alert('密碼不匹配，請重新輸入');
        return;
    }

    try {
        const response = await fetch("http://localhost:5100/DermSight/User/Register", {
            method: 'POST',
            body: formData
        });

        const result = await response.json();

        if (response.ok) {
            console.log("註冊成功", result);
            alert('註冊成功！請收信。');
            window.location.href = 'verify.html';
        } else {
            console.error('註冊失敗:', result);
            alert(result.message || '註冊失敗');
        }
    } catch (error) {
        console.error('發生錯誤:', error);
        alert('發生錯誤，請稍後再試');
    }
}

// 登入
async function login(event) {
    event.preventDefault();

    const form = document.getElementById('login-form');
    const formData = new FormData(form);

    // 简单的验证逻辑
    const data = {
        Account: formData.get('Account'),
        Password: formData.get('Password'),
    };

    if (!data.Account || !data.Password) {
        alert("請確切輸入登入資料");
        return;
    }

    try {
        const response = await fetch("http://localhost:5100/DermSight/User/Login", {
            method: 'POST',
            body: formData
        });

        const result = await response.json();
        if (response.ok) {
            localStorage.setItem('accessToken', result.data); // 假设返回的数据中有 accessToken
            alert(result.message);

            if (result.data.permission === '4') {
                window.location.href = '../admin/index.html';
            } else if (result.data.permission === '1') {
                window.location.href = '../index/index.html';
            } else {
                window.location.href = '../index/index.html';
            }
        } else {
            console.error('登入失敗:', result);
            alert(result.message || '登入失敗');
        }
    } catch (error) {
        console.error('發生錯誤:', error);
        alert('發生錯誤，請稍後再試');
    }
}

// 忘記密碼
async function forget(event) {
    event.preventDefault();

    const form = document.getElementById('forget-form');
    const data = new FormData(form);

    const token = localStorage.getItem('accesstoken');
    try {
        const response = await fetch("http://localhost:5100/DermSight/User/ForgetPassword", {
            method: 'POST',
            body: data
        });

        const result = await response.json();
        
        console.log('Response:', result);

        if (response.ok) {
            localStorage.setItem('accesstoken',result.data);
            alert("驗證成功！請收信。");
            showForgetAuthcode();
        } else {
            alert(result.message || '驗證失敗');
        }
    } catch (error) {
        console.error('發生錯誤:', error);
        alert('發生錯誤，請稍後再試');
    }
}

// 忘記密碼 驗證碼
async function forget_authcode(event) {
    event.preventDefault();

    const mail = document.querySelector('#forget-form input[name="Mail"]').value;
    const form = document.getElementById('forget-authcode-form');
    const data = new FormData(form);

    data.append('Mail', mail);

    try {
        const response = await fetch("http://localhost:5100/DermSight/User/CheckForgetPasswordCode", {
            method: 'POST',
            body: data
        });

        const result = await response.json();
        
        if (response.ok) {
            localStorage.setItem('accesstoken',result.data)
            alert("驗證成功！請輸入新密碼！");
            showForgetChangepassword();
        } else {
            alert(result.message || '驗證失敗');
            window.location.href = 'verify.html';
        }
    } catch (error) {
        console.error('發生錯誤:', error);
        alert('發生錯誤，請稍後再試');
    }
}

// 忘記密碼 更改密碼
async function forget_changepassword(event) {
    event.preventDefault();

    const mail = document.querySelector('#forget-form input[name="Mail"]').value;
    const form = document.getElementById('forget-changepassword-form');
    const formData = new FormData(form);

    formData.append('Mail', mail); // 将邮箱地址添加到 FormData 对象中

    try {
        const token = localStorage.getItem('accesstoken');

        const response = await fetch("http://localhost:5100/DermSight/User/ChangePasswordByForget", {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`
            },
            body: formData
        });

        const result = await response.json();

        if (response.ok) {
            alert("密碼修改成功！請重新登入！");
            window.location.href = 'verify.html';
        } else {
            alert(result.message || '密碼修改失敗');
        }
    } catch (error) {
        console.error('發生錯誤:', error);
        alert('發生錯誤，請稍後再試');
    }
}
