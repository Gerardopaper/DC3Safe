$(document).ready(function () {
    createDataTable({
        tableId: "datatable",
        dataUrl: "/Companies/List",
        buttonsId: "datatables-buttons",
        batchDeleteUrl: "/Companies/BatchDelete",
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
                                <a href="/Companies/Details/${row["id"]}">${data}</a>
                           `;
                }
            },
            { data: "shcp" },
            {
                data: 'id',
                render: function (data) {
                    return `
                                <a href="/Companies/Edit/${data}" class="btn btn-sm btn-primary"><i class="fa-solid fa-pen"></i> Editar</a>
                                <a href="/Companies/Delete/${data}" class="btn btn-sm btn-danger"><i class="fa-solid fa-trash"></i> Eliminar</a>
                           `;
                }
            }
        ]
    });
});