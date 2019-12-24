$(function () {
    $(function () {
        $.validator.methods.date = function (value, element) {
            return this.optional(element) || moment(value, "DD/MM/YYYY HH:mm", true).isValid();
        }
    });
});