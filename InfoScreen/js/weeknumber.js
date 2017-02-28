function addWkColumn(tblId, wkStart) {
    var tbl = document.getElementById(tblId);
    var tblBodyObj = tbl.tBodies[0];

    for (var i = 0; i < tblBodyObj.rows.length; i++) {
        // Month Header
        if (i == 0) {
            // Add extra colspan column
            tblBodyObj.rows[i].cells[0].colSpan = 8;
        }
        // Week Header
        if (i == 1) {
            // Add week column headline
            var newCell = tblBodyObj.rows[i].insertCell(0);
            newCell.innerHTML = 'Uge';
            newCell.className = 'event-calendar-week';
        }
        // Normal row
        if (i >= 2) {
            // Add the weeknumbers 
            var newCell = tblBodyObj.rows[i].insertCell(0);
            newCell.innerHTML = wkStart;
            wkStart += 1;
            if (wkStart == 53) {
                wkStart = 1;
            }
            newCell.className = 'event-calendar-week'
        }
    }
}