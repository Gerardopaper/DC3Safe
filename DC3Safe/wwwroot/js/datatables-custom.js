function createDataTable(options) {
    var table = $('#' + options.tableId).DataTable({
        ajax: options.dataUrl,
        processing: true,
        serverSide: true,
        buttons: [
            {
                extend: 'copy',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'csv',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'excel',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'pdf',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: ':visible'
                }
            },
            "colvis"
        ],
        language: {
            url: '/lib/DataTables/plugins/i18n/es-MX.json',
        },
        columns: options.columns,
        initComplete: function () {
            table.buttons().container().appendTo($('#' + options.buttonsId));
            var buttons = document.querySelectorAll(".dt-buttons button");
            for (var i = 0; i < buttons.length; i++) {
                buttons[i].classList.remove("btn-secondary");
                buttons[i].classList.add("btn-sm", "btn-outline-primary");
            }
            buttons[0].innerHTML = `<i class="fa-regular fa-copy"></i> ${buttons[0].innerHTML}`;
            buttons[1].innerHTML = `<i class="fa-solid fa-file-csv"></i> ${buttons[1].innerHTML}`;
            buttons[2].innerHTML = `<i class="fa-regular fa-file-excel"></i> ${buttons[2].innerHTML}`;
            buttons[3].innerHTML = `<i class="fa-solid fa-file-pdf"></i> ${buttons[3].innerHTML}`;
            buttons[4].innerHTML = `<i class="fa-solid fa-print"></i> ${buttons[4].innerHTML}`;
            buttons[5].innerHTML = `<i class="fa-regular fa-eye"></i> ${buttons[5].innerHTML}`;

            options.callback ? options.callback() : null;
        }
    });

    document.getElementById("batch-delete").addEventListener("click", async () => {
        var elements = document.querySelectorAll('.table-check:checked');
        if (elements.length == 0) {
            showToast("Selecctiona al menos un registro.");
            return;
        }

        var ids = [];
        for (var i = 0; i < elements.length; i++) {
            ids.push(elements[i].value);
        }

        var result = await batchDeleteAsync(ids);
        if (result) {
            table.ajax.reload();
            showToast("Registros eliminados.");
        } else {
            showToast("Error al intentar eliminar registros.");
        }
    });

    async function batchDeleteAsync(ids) {
        try {
            var data = await fetch(options.batchDeleteUrl, {
                headers: {
                    "Content-Type": "application/json",
                    "X-CSRF-TOKEN": document.querySelector('[name=__RequestVerificationToken]').value
                },
                method: "POST",
                body: JSON.stringify(ids)
            });

            return data.ok;
        } catch (error) {
            console.error(`Error tryign to call ${options.batchDeleteUrl}`, error);
        }
        return false;
    }

    document.getElementById("batch-delete-modal-btn").addEventListener("click", () => {
        if (document.querySelectorAll('.table-check:checked').length == 0) {
            showToast("Selecctiona al menos un registro.");
            return;
        }
        var modal = bootstrap.Modal.getOrCreateInstance(document.getElementById("batch-delete-modal"));
        modal.show();
    });

    return table;
}

function selectAllTableChecks(check) {
    let tableChecks = document.getElementsByClassName("table-check");
    for (let i = 0; i < tableChecks.length; i++) {
        tableChecks[i].checked = check.checked;
    }
}