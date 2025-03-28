$(document).ready(function () {
    createDataTable({
        tableId: "datatable",
        dataUrl: "/ProgramsInformation/List",
        buttonsId: "datatables-buttons",
        batchDeleteUrl: "/ProgramsInformation/BatchDelete",
        columns: [
            {
                data: 'id',
                render: function (data) {
                    return `
                                <input type="checkbox" class="form-check-input table-check" value="${data}" />
                            `;
                }
            },
            {
                data: 'name',
                render: function (data, type, row) {
                    return `
                                <a href="/ProgramsInformation/Details/${row["id"]}">${data}</a>
                           `;
                }
            },
            { data: 'duration' },
            { data: 'start_date' },
            { data: 'end_date' },
            { data: 'category' },
            {
                data: 'id',
                render: function (data) {
                    return `
                                <a href="/ProgramsInformation/Edit/${data}" class="btn btn-sm btn-primary"><i class="fa-solid fa-pen"></i> Editar</a>
                                <a href="/ProgramsInformation/Delete/${data}" class="btn btn-sm btn-danger"><i class="fa-solid fa-trash"></i> Eliminar</a>
                           `;
                }
            }
        ],
        callback: function () {
            document.getElementById("dt-search-0").placeholder = "Nombre";
        }
    });
});