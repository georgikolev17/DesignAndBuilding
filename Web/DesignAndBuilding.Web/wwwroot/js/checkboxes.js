$('#all-checkbox').click(() => {
    let activeAssignments = document.querySelectorAll('#active');
    let finishedAssignments = document.querySelectorAll('#finished');
    for (let a of activeAssignments) {
        a.style.display = '';
    }
    for (let a of finishedAssignments) {
        a.style.display = '';
    }
});
$('#active-checkbox').click(active);

$('#finished-checkbox').click(() => {
    let activeAssignments = document.querySelectorAll('#active');
    let finishedAssignments = document.querySelectorAll('#finished');
    for (let a of activeAssignments) {
        a.style.display = 'none';
    }
    for (let a of finishedAssignments) {
        a.style.display = '';
    }
});

function active() {
    let activeAssignments = document.querySelectorAll('#active');
    let finishedAssignments = document.querySelectorAll('#finished');
    for (let a of activeAssignments) {
        a.style.display = '';
    }
    for (let a of finishedAssignments) {
        a.style.display = 'none';
    }
}

active();