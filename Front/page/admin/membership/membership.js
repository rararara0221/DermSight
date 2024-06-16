document.addEventListener('DOMContentLoaded', (event) => {
    // 获取模态窗口
    const modal = document.getElementById('newsModal');
    const modalTitle = document.getElementById('modalTitle');
    const modalContent = document.getElementById('modalContent');

    // 获取打开模态窗口的按钮
    const btns = document.getElementsByClassName('openModalBtnNews');

    // 获取关闭按钮
    const closeButton = modal.getElementsByClassName('close')[0];

    // 当点击按钮时，打开模态窗口并更新内容
    for (let btn of btns) {
        btn.onclick = function(event) {
            event.preventDefault();  // 阻止默认行为（链接跳转）
            modalTitle.innerText = btn.getAttribute('data-title');
            modalContent.innerText = btn.getAttribute('data-content');
            modal.style.display = 'block';
        }
    }

    // 当点击关闭按钮时，关闭模态窗口
    closeButton.onclick = function() {
        modal.style.display = 'none';
    }

    // 当点击模态窗口外部时，关闭模态窗口
    window.onclick = function(event) {
        if (event.target === modal) {
            modal.style.display = 'none';
        }
    }
});