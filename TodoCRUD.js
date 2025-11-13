const token = localStorage.getItem("token");
const todoList = document.getElementById("todo-table");
const todoModal = new bootstrap.Modal(document.getElementById("todoModal"));
const todoTitle = document.getElementById("todoTitle");
const todoDone = document.getElementById("todoDone");
const todoId = document.getElementById("todoId");
const saveTodoBtn = document.getElementById("saveTodoBtn");
let isEditing = false;

function updateCRUDUI() {
    const roles = JSON.parse(localStorage.getItem("roles")) || [];
    const btnCreate = document.getElementById("btnCreate");

    if (roles.includes("Admin")) 
    {
        btnCreate.classList.remove("d-none");
    }
    else {
        btnCreate.classList.add("d-none");
    }

    document.querySelectorAll("#todo-table .btn-success, #todo-table .btn-danger").forEach(btn => 
    {
        if (roles.includes("Admin")) {
            btn.classList.remove("d-none");
        } else {
            btn.classList.add("d-none");
        }
    });
}

function openCreateModal() {
    isEditing = false;
    document.getElementById("todoModalLabel").textContent = "Create new item";
    todoTitle.value = "";
    todoDone.checked = false;
    todoId.value = "";
    todoModal.show();
}

function openEditModal(id, title, isDone) {
    isEditing = true;
    document.getElementById("todoModalLabel").textContent = "Edit item";
    todoTitle.value = title;
    todoDone.checked = isDone;
    todoId.value = id;
    todoModal.show();
}

async function deleteItem(id) {
    const result = await Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, delete it!'
    });
    if (result.isConfirmed)
    {
        const deleteBtn = document.querySelector(`button[onclick="deleteItem(${id})"]`);
        deleteBtn.disabled = true;

        try {
            await axios.delete(
                `https://localhost:7181/api/ToDoItem/${id}`,
                {
                    headers: { Authorization: `Bearer ${token}`}
                }
            );
            Swal.fire(
                'Deleted!',
                'Your item has been deleted.',
                'success'
            );
            getList();
        } catch (error) {
            Swal.fire(
                'Failed!',
                'Could not delete item.',
                'error'
            );
            console.error(error);
        } finally {
            deleteBtn.disabled = false;
        }
    }

}

saveTodoBtn.addEventListener("click", async () => {
    const title = todoTitle.value.trim();
    if (title === "") {
        alert("Title cannot be empty!");
        return;
    }

    const data = {
        title: title,
        isDone: todoDone.checked
    };

    saveTodoBtn.disabled = true;

    try {
        if (isEditing) {
            const id = todoId.value;
            await axios.put(
                `https://localhost:7181/api/ToDoItem/${id}`,
                {id, ...data },
                {
                    headers: { Authorization: `Bearer ${token}`}
                }
            );
            Swal.fire({
                icon: 'success',
                title: 'Updated!',
                text: 'Item updated successfully'
            });
        } else {
            await axios.post(
                "https://localhost:7181/api/ToDoItem",
                data,
                {
                    headers: { Authorization: `Bearer ${token}`}
                }
            );
            Swal.fire({
                icon: 'success',
                title: 'Created!',
                text: 'Item created successfully'
            });
        }
        todoModal.hide();
        getList();
    } catch (error) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Operation failed!'
            });
        console.error(error);
    } finally {
        saveTodoBtn.disabled = false;
    }
});

async function getList() {
    try {
        const response = await axios.get(
            "https://localhost:7181/api/ToDoItem",
            {
                headers: { Authorization: `Bearer ${token}`}
            }
        );
        const list = response.data;

        todoList.innerHTML = "";

        if (list.length === 0) 
        {
            todoList.innerHTML = '<tr><td colspan="3">No items found</td></tr>';
        } 
        else 
        {
            list.forEach(item =>{
                const tr = document.createElement("tr");
                tr.innerHTML = `
                    <td>${item.title}</td>
                    <td>${item.isDone ? 'Done' : 'Pending'}</td>
                    <td>
                        <button class="btn btn-success" onclick="openEditModal(${item.id}, '${item.title.replace(/'/g, "\\'")}', ${item.isDone})">
                            <i class="bi bi-pencil"></i> Edit
                        </button>
                        <button class="btn btn-danger" onclick="deleteItem(${item.id})">
                            <i class="bi bi-trash"></i> Delete
                        </button>
                    </td>`;
                todoList.appendChild(tr);
            });
        }

        updateCRUDUI();
    } 
    catch (error) 
    {
        todoList.innerHTML = `<tr><td colspan="3">${error.message}</td></tr>`;
        console.error(error);
    }
}