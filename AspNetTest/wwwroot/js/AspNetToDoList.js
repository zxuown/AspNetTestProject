const localeUk = {
    daysMin: ["Нд", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"],
    months: [
        "Січ",
        "Лютий",
        "Бер",
        "Кві",
        "Тра",
        "Чер",
        "Лип",
        "Сер",
        "Вер",
        "Жов",
        "Лис",
        "Гру"
    ],
    monthsShort: ["Січ",
        "Лют",
        "Бер",
        "Кві",
        "Тра",
        "Чер",
        "Лип",
        "Сер",
        "Вер",
        "Жов",
        "Лис",
        "Гру"],
    pickers: ["наст. 7 днів", "наст. 30 днів", "мин. 7 днів", "мин. 30 днів"],
    placeholder: {
        date: "Оберіть дату",
        dateRange: "Оберіть період"
    }
}
let airDatepicker = new AirDatepicker('#dateInput', {
    navTitles: {
        days(dp) {
            if (dp.selectedDates.length) {
                let date = dp.selectedDates[0];
                return `<small>Ви вибрали: ${dp.formatDate(date, 'dd MMMM yyyy')}</small>`;
            }
            return 'Оберіть дату';
        }
    },
    locale: localeUk,
    onSelect: updateDisplayedTasks
});
let data = [];
//const savedData = localStorage.getItem('todoData');
//if (savedData) {
//    data = JSON.parse(savedData);
//}
//function updateLocalStorage() {
//    localStorage.setItem('todoData', JSON.stringify(data));
//}

const form = document.getElementById('todoForm');
const titleInput = document.getElementById('titleInput');
const dateInput = document.getElementById('dateInput');
const tableBody = document.getElementById('todoList');
const modal = document.getElementById("modal");
const modalText = document.getElementById("modal-text");
const modalConfirmButton = document.getElementById("modal-confirm");
const modalCancelButton = document.getElementById("modal-cancel");
function createRow(task) {
    console.log(task)
    const row = document.createElement('tr');
    const checkboxCell = document.createElement('td');

    const checkbox = document.createElement('input');
    checkbox.type = 'checkbox';
    checkbox.checked = task.done;
    checkbox.addEventListener('change', function () {
        task.done = this.checked;
        if (this.checked)
        {
            fetch(`/ToDoList/SetCompleted/${task.id}`,
                {
                    method: "post"
                }
            ).then(async () => {
                row.classList.add('completed');
            })

        }
        else
        {
            fetch(`/ToDoList/SetNotCompleted/${task.id}`,
                {
                    method: "post"
                }
            ).then(async () => {
                row.classList.remove('completed');
            })
           
        }
    });
    checkboxCell.appendChild(checkbox);
    row.appendChild(checkboxCell);

    const titleCell = document.createElement('td');
    const titleText = document.createElement('span');
    const titleInput = document.createElement('input');
    titleText.textContent = task.title;
    titleInput.type = 'text';
    titleInput.value = task.title;
    titleInput.style.display = 'none';
    titleCell.appendChild(titleText);
    titleCell.appendChild(titleInput);
    titleCell.addEventListener('click', function () {
        titleText.style.display = 'none';
        titleInput.style.display = 'inline-block';
        titleInput.focus();

    });
    titleInput.addEventListener('blur', function () {
        const newTitle = titleInput.value.trim();
        if (newTitle !== '') {
            task.title = newTitle;
            titleText.textContent = newTitle;
            fetch(`/ToDoList/Update/${task.id}`, {
                method: "post",
                headers: {
                    'Content-Type': "application/json"
                },
                body: JSON.stringify(task)
            }).then(async () => {
                while (tableBody.firstChild) {
                    tableBody.removeChild(tableBody.firstChild);
                }
                getTasks(currentDate.toLocaleString()).then(list => {
                    list.forEach(task => createRow(task))
                })

            })
        }
        titleInput.style.display = 'none';
        titleText.style.display = 'inline';
    });
    row.appendChild(titleCell);

    const dateCell = document.createElement('td');
    dateCell.textContent = task.date;
    row.appendChild(dateCell);

    const deleteCell = document.createElement('td');
    const deleteButton = document.createElement('button');
    deleteButton.style.background = "red";
    deleteButton.textContent = 'Видалити';
    deleteButton.addEventListener('click', function () {
        showModal("Ви впевнені, що бажаєте видалити?", function (result) {
            if (result) {
                console.log(task)
                fetch(`/ToDoList/Delete/${task.id}`, {
                    method: "delete",
                }).then(async () => {
                    tableBody.removeChild(row);
                    //const index = data.indexOf(task);
                    //data.splice(index, 1);
                })    
            }
        });
    });
    deleteCell.appendChild(deleteButton);
    row.appendChild(deleteCell);

    if (task.done) {
        row.classList.add('completed');
    }
    tableBody.appendChild(row);
}
function getTasks(dataStr) {
    return fetch(`/ToDoList/List/${dataStr}`).then(r => r.json());
}

var currentDate

function init() {
    getTasks(new Date().toLocaleString()).then(list => {
        list.forEach(task => createRow(task))
    })
}

function updateDisplayedTasks(selectedDates, dateStr) {
    console.log(selectedDates);
    currentDate =  selectedDates.date
    const filterDate = selectedDates.formattedDate;
    while (tableBody.firstChild) {
        tableBody.removeChild(tableBody.firstChild);
    }
    getTasks(filterDate).then(list => {
        list.forEach(task=>createRow(task))
    })
}

airDatepicker.on('change', updateDisplayedTasks);

form.addEventListener('submit', function (event) {
    event.preventDefault();
    const title = titleInput.value;
    const date = dateInput.value;

    if (title && date) {
        const task =
        {
            title: title,
            //"16.09.2023" => "2023-09-16"
            date: currentDate.toISOString(),
            done: false
        };
        fetch("/ToDoList/Add", {
            method: "post",
            headers: {
                'Content-Type': "application/json"
            },
            body: JSON.stringify(task)
        }).then(async() => {
            while (tableBody.firstChild) {
                tableBody.removeChild(tableBody.firstChild);
            }
            getTasks(currentDate.toLocaleString()).then(list => {
                list.forEach(task => createRow(task))
            })

        })
        titleInput.value = '';
    }
});


function deleteCompleted() {
    showModal("Ви впевнені, що бажаєте видалити усі виконані справи?", function (result) {
        if (result) {
           
                fetch(`/ToDoList/DeleteAllDone/${currentDate.toLocaleString()}`, {
                    method: "delete",
                }).then(async () => {
                    while (tableBody.firstChild) {
                        tableBody.removeChild(tableBody.firstChild);
                    }
                    getTasks(currentDate.toLocaleString()).then(list => {
                        list.forEach(task => createRow(task))
                    })
                    
                })
        }

    });

}
function showModal(message, callback) {
    modalText.textContent = message;
    modal.style.display = "block";

    modalConfirmButton.onclick = function () {
        callback(true);
        modal.style.display = "none";
    };

    modalCancelButton.onclick = function () {
        callback(false);
        modal.style.display = "none";
    };
}

//let sortDirections =
//{
//    completed: 'asc',
//    title: 'asc',
//    date: 'asc'
//};

//function sortTable(column) {
//    data.sort(function (a, b) {
//        if (column === 'completed') {
//            return (a.completed === b.completed) ? 0 : (a.completed ? -1 : 1);
//        }
//        else if (column === 'title') {
//            return a.title.localeCompare(b.title);
//        }
//        else if (column === 'date') {
//            return new Date(a.date) - new Date(b.date);
//        }
//    });

//    if (sortDirections[column] === 'asc') {
//        sortDirections[column] = 'desc';
//    }
//    else {
//        sortDirections[column] = 'asc';
//        data.reverse();
//    }

//    while (tableBody.firstChild) {
//        tableBody.removeChild(tableBody.firstChild);
//    }

//    data.forEach(createRow);
//}

init()