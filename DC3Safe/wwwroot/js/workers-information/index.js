$(document).ready(function () {
    createDataTable({
        tableId: "datatable",
        dataUrl: "/WorkersInformation/List",
        buttonsId: "datatables-buttons",
        batchDeleteUrl: "/WorkersInformation/BatchDelete",
        columns: [
            {
                data: 'id',
                render: function (data) {
                    return `
                                <input type="checkbox" class="form-check-input table-check" value="${data}" />
                            `;
                }
            },
            { data: 'last_name' },
            { data: 'last_name2' },
            { data: 'first_name' },
            {
                data: 'curp',
                render: function (data, type, row) {
                    return `
                                <a href="/WorkersInformation/Details/${row["id"]}">${data}</a>
                           `;
                }
            },
            { data: 'occupation' },
            { data: 'position' },
            {
                data: 'id',
                render: function (data) {
                    return `
                                <a href="/WorkersInformation/Edit/${data}" class="btn btn-sm btn-primary"><i class="fa-solid fa-pen"></i> Editar</a>
                                <a href="/WorkersInformation/Delete/${data}" class="btn btn-sm btn-danger"><i class="fa-solid fa-trash"></i> Eliminar</a>
                           `;
                }
            }
        ]
    });
});