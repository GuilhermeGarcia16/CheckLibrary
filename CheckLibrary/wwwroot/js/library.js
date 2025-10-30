
//statement variables
let sectionLibraries = $('#sec-libraries');
let sectionCreateNewLibrary = $('#sec-create-new-library');
let sectionSearchBook = $('#sec-search-book');
let btnNewList = $('#btn-new-list');
let btnNextName = $('#nextName');
let btnBackName = $('#backName');
let btnBackSearchBook = $('#backSearchBook');
let wordSearch = $('[name="wordSearch"]');
let inputName = $('#inputName');
let listGroupBooks = $('#list-result-search');
let secListGroup = $('#sec-list-group-add');
let listIdBook = [];
let btnSave = $('#btn-save-library');

//Load page
sectionCreateNewLibrary.hide();
sectionSearchBook.hide();
listGroupBooks.hide();
secListGroup.hide();

//Event add elements
btnNewList.on('click', function () {
    sectionCreateNewLibrary.show();
    $(this).hide();
});

btnNextName.on('click', function () {
    if (inputName.val() != '' || inputName.val() == '') {
        sectionSearchBook.show();
        btnBackName.hide();
        inputName.attr('disabled', 'disabled');
        $(this).hide();
    }
});

btnBackName.on('click', function () {
    sectionCreateNewLibrary.hide();
    btnNewList.show();
});

inputName.on('keydown', function (event) {
    if (event.key == 'Backspace' && event.target.value.length <= 1) {
        btnNextName.attr('disabled', 'true');
    } else if (btnNextName.is(':disabled')) {
        btnNextName.removeAttr('disabled');
    }
});

btnBackSearchBook.on('click', function () {
    sectionSearchBook.hide();
    btnNextName.show();
    btnBackName.show();
    inputName.removeAttr('disabled');
});

btnSave.on('click', function () {
    jsonData = { "Title": inputName.val(), "ListBooks": JSON.stringify(listIdBook) }
    $.post("Library/Create", jsonData, function(response) {
        alert(response.message);
        window.location = response.redirect;
    }).fail(function () {
        alert("Error");
    });
});

function addBookTempList(bookId) {
    let listTemp = $('#list-books-temp');
    let title = $(`#${bookId}>span`)[0].textContent;
    let btnClose = $('<button type="button" class="btn-close" aria-label="Close" onclick="verifyListBook()">');
    let position = listIdBook.indexOf(bookId);

    if (position == -1) {
        listIdBook.push(bookId);
        position = listIdBook.indexOf(bookId);

        btnClose.on("click", function () {
            $(this).parent()[0].remove();
            listIdBook.splice(listIdBook.indexOf(position), 1);
        });
        let item = $('<li class="list-group-item">').text(title).append(btnClose);
        listTemp.append(item);

        if (!secListGroup.is(':visible') && listTemp.length > 0) {
            secListGroup.show();
            btnSave.show();
        }
    } else {
        alert("Book already selected");
    }
}
function verifyListBook() {
    if (listIdBook.length == 1) {
        secListGroup.hide();
        btnSave.hide();
    }
}

//AJAX
wordSearch.on('keypress', function () {
    if ($(this).val().length > 2) {
        $.ajax({
            url: "/Home/SearchToLibrary",
            data: { wordSearch: $(this).val() },
            success: function (books) {
                listGroupBooks.empty();
                if (books.length > 0) {
                    booksObj = JSON.parse(books);
                    booksObj.forEach(function (book) {
                        let text = '<li class="list-group-item" id="' + book.Id + '"><span>' + book.Title + '</span>\
                                            <button type="button" id="btnAdd" class="btn btn-warning" onclick="addBookTempList('+ book.Id + ')">Add</button></li>';
                        listGroupBooks.append(text);
                        listGroupBooks.show();
                    });
                }
            }
        });
    }
    else {
        listGroupBooks.empty();
    }
});