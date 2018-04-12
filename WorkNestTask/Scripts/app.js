var app = new Vue({
    el: '#app',
    data: {
        editMode: false,
        editedTask: {id: NaN },
        token: '',
        userName: '',
        password: '',
        task: { name: '', assigneeId: '' },
        columns: [
            'Name',
            'Created At',
            'Created By',
            'AssigneeTo',
            'Completed At',
            '', ''
            ],
        results: [],
        users:[]
    },
    methods: {
        login: function (event) {
            axios({
                method: 'post',
                url: '/api/signin',
                data: {
                    userName: this.$data.userName,
                    password: this.$data.password,
                }
            }).then(response => {
                this.$data.token = 'Bearer ' + response.data.token;
                this.getTasks();
                this.getUsers();
            })
        },
        getTasks: function () {
            axios.get('/api/tasks', {
                headers: { 'Authorization': this.$data.token },
            }).then(response => {
                this.$data.results = response.data;
                });

        },

        getUsers: function () {
            axios.get('/api/users', {
                headers: { 'Authorization': this.$data.token },
            }).then(response => {
                this.$data.users = response.data;
            });
        }, 

        addTask: function () {
            axios.post('/api/tasks', this.task, {
                headers: { 'Authorization': this.$data.token }
            }).then(response => {
                this.getTasks();
            });
        },

        deleteTask: function (id) {
            axios.delete('/api/tasks/' + id, {
                headers: { 'Authorization': this.$data.token }
            }).then(response => {
                this.getTasks();
            });
        },

        updateTask(task) {
            axios.put('/api/tasks/' + task.id,
                {
                    name: task.name,
                    assigneeId: task.assign.id,
                    completed: task.completed
                },
                {
                    headers: { 'Authorization': this.$data.token }
                }).then(response => {
                    this.cancelEdit();
                    this.getTasks();
                });
        },

        editTask(task) {
            this.beforEditCache = task;
            this.$data.editedTask = task
        },

        cancelEdit() {
            //this.beforEditCache = task;
            this.$data.editedTask = { id: NaN }
        }
    }
});

Vue.filter('formatDate', function (value) {
    if (value) {
        return moment(String(value)).format('DD.MM.YYYY hh:mm')
    }
});