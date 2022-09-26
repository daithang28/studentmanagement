$(document).ready(function () {
    $('li').click(function () {
        $('li').removeClass('active');
        $(this).addClass('active');
    })
});


var myId = '';
function loadData(idAccount) {
    getElementById(idAccount);
    myId = idAccount;
    $.ajax({
        url: '/Tutor/TutorChat/ListChat?friendId=' + idAccount,
        type: 'GET',
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var myChatBox = $('.chatContainerScroll');
            myChatBox.html('');
            var html = '';
            $.each(result, function (index, item) {
                var myDate = null;
                if (item.createdDate != null) {
                    var dateStr = item.createdDate.slice(item.createdDate.indexOf('(') + 1, item.createdDate.length - 3);
                    console.log(dateStr);
                    var current = new Date();
                    current.setTime(Number(dateStr));
                    console.log(current.getTime());

                    myDate = current.getHours() + ": " + current.getMinutes();
                    console.log(myDate);
                }
               
                if (item.isSent) {
                    html = "<li class='chat-right'>"
                        + "<div class='chat-text'>"
                        + item.chatContent
                        + "</div>"
                        + "<div class='chat-avatar'>"
                        + "<img src='https://www.bootdey.com/img/Content/avatar/avatar3.png' alt='Retail Admin'>"
                        + "<div class='chat-name'>" + myDate + "</div>"
                        + "</div>"
                        + "</li>";

                } else {
                    html = "<li class='chat-left'>"
                        + "<div class='chat-avatar'>"
                        + "<img src='https://www.bootdey.com/img/Content/avatar/avatar3.png' alt='Retail Admin'>"
                        + "<div class='chat-name'>" + myDate + "</div>"
                        + "</div>"
                        + "<div class='chat-text'>"
                        + item.chatContent
                        + "</div>"
                        + "</li>";
                }
                myChatBox.append(html);
                $('textarea').val('').focus();
            });
        }
    })
}

function getElementById(idAccount) {
    var displayName = $('.myName');
    var name = document.querySelectorAll('.name');
    for (var i = 0; i < name.length; i++) {
        if (name[i].className == ("name Name_" + idAccount)) {
            console.log(name[i].textContent);
            displayName.text(name[i].textContent);
            return;
        }
    }
}

var chatHub = $.connection.chattingHub;
chatHub.client.chatForFun = function (someText) {
    var current = new Date();
    var encodedText = $('textarea').text(someText).html();

    var html = "<li class='chat-right'>"
        + "<div class='chat-text'>"
        + encodedText
        + "</div>"
        + "<div class='chat-avatar'>"
        + "<img src='https://www.bootdey.com/img/Content/avatar/avatar3.png' alt='Retail Admin'>"
        + "<div class='chat-name'>" + current.getHours() + ": " + current.getMinutes() + "</div>"
        + "</div>"
        + "</li>"

    $('.chatContainerScroll').append(html);
    $('textarea').val('').focus();
};



$.connection.hub.start()
    .done(function () {
        $('.send_btn').click(function () {
            debugger
            var someText = $('textarea').val();
            if (someText.trim() != '') {
                chatHub.server.chatting(someText);

                $('textarea').val('').focus();
                $.ajax({
                    url: '/Tutor/TutorChat/SaveChat?accountId=' + myId + '&chatContent=' + someText,
                    type: 'GET',
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        alert('Success!!');
                    }
                });
            }
        });
    })
    .fail();