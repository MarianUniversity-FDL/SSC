// Gets select elements for grade and then converts to array, create options array
// Goes through each select element with out looper
// Inner loop wraps elements of options array in options tag, then adds to select element
var select = Array.from(document.getElementsByClassName('grade'));
var options = ["Select", "A", "A-", "B+", "B", "B-", "C+", "C", "C-", "D+", "D", "D-", "F"];
var optValue = [0, 4.0, 3.67, 3.33, 3.0, 2.67, 2.33, 2.0, 1.67, 1.33, 1.0, 0.67, 0.0];
for (var i = 0; i < select.length; i++) {
    for (var j = 0; j < options.length; j++) {
        var option = document.createElement('option');
        option.text = options[j];
        option.value = optValue[j];
        select[i].append(option);
    }
}

// Help field set to default hidden in css
// Button toggles it to hide or show
function onHelpClick() {
    var toggleHelp = document.getElementById('help');
    if (toggleHelp.style.display === 'none') {
        toggleHelp.style.display = 'block';
    } else {
        toggleHelp.style.display = 'none';
    }
}

// Gets button index, then get tbody elements and converts them to array
// Then checks row count else gets parent row and removes
function delRow(button) {
    var tableBody = document.getElementById('tbody');
    var bodyRows = Array.from(tableBody.getElementsByTagName('tr'));
    if (bodyRows.length < 2) {
        alert("There must be at least 1 row for GPA Calculation!");
    } else {
        var tr = button.parentNode.parentNode;
        tr.parentNode.removeChild(tr);
    }
    updateRowIDs();
}

// Gets first row of table, clones it, resets it, and then adds it to table.
function insRow() {
    var tableBody = document.getElementById('tbody')
    var firstRow = tableBody.getElementsByTagName('tr')[0];
    var clonedRow = firstRow.cloneNode(true);
    clonedRow.getElementsByClassName('course')[0].value = "";
    clonedRow.getElementsByClassName('credit')[0].value = "";
    clonedRow.getElementsByClassName('grade')[0].SelectedValue = "Select";
    tableBody.appendChild(clonedRow);
    updateRowIDs();
}

// Updates element IDs for inputs after row add/delete
function updateRowIDs() {
    var rowList = document.getElementById('tbody').getElementsByTagName('tr');
    var courseList = document.getElementsByClassName('course');
    var creditList = document.getElementsByClassName('credit');
    var gradeList = document.getElementsByClassName('grade');

    for (var i = 0; i < rowList.length; i++) {
        courseList[i].id = "course" + i;
        creditList[i].id = "credit" + i;
        gradeList[i].id = "grade" + i;
    }
}

// Clears inputs
function clrAll() {
    document.getElementById('curCredit').value = "";
    document.getElementById('curGPA').value = "";
    document.getElementById('calGPA').value = "";
    var rowList = document.getElementById('tbody').getElementsByTagName('tr');
    var courseList = document.getElementsByClassName('course');
    var creditList = document.getElementsByClassName('credit');
    var gradeList = document.getElementsByClassName('grade');

    for (var i = 0; i < rowList.length; i++) {
        courseList[i].value = "";
        creditList[i].value = "";
        gradeList[i].SelectedValue = "Select";
        gradeList[i].value = 0;
    }
}

// Grabs every field value except course, looping through rows
// and performs calculations and then outputs to calculated field
function calculateGPA() {
    var curCredits = document.getElementById('curCredit').value;
    var curGPA = document.getElementById('curGPA').value;
    var calGPA = document.getElementById('calGPA');
    var rowList = document.getElementById('tbody').getElementsByTagName('tr');
    var creditList = document.getElementsByClassName('credit');
    var gradeList = document.getElementsByClassName('grade');
    var sumGrade = 0, sumCredit = 0, ttlGrade = 0, ttlCredit = 0, prodCurGrade = 0;

    for (var i = 0; i < rowList.length; i++) {
        sumGrade += (Number(creditList[i].value) * Number(gradeList[i].value));
        sumCredit += Number(creditList[i].value);
    }

    prodCurGrade = (Number(curCredits) * Number(curGPA));
    ttlGrade = sumGrade + prodCurGrade;
    ttlCredit = (sumCredit + Number(curCredits));
    calGPA.value = Number(ttlGrade / ttlCredit).toFixed(2);

    if (isNaN(calGPA.value)) {
        calGPA.value = "ERROR";
        calGPA.style.color = "red";
        alert("Please verify that all used rows are properly filled in or selected.");
    } else if (calGPA.value < 2.00) {
        calGPA.style.color = "red";
    } else if (calGPA.value > 2.00 && calGPA.value < 3.00) {
        calGPA.style.color = "yellow";
    } else {
        calGPA.style.color = "green";
    }
}