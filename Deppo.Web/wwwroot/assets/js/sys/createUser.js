﻿"use strict";
var KTUsersAddUser = (function () {
    const modal = document.getElementById("kt_modal_add_user"),
        form = modal.querySelector("#kt_modal_add_user_form"),
        modalInstance = new bootstrap.Modal(modal);

    return {
        init: function () {
            (() => {
                var formValidator = FormValidation.formValidation(form, {
                    fields: {
                        firstName: {
                            validators: { notEmpty: { message: "Ad gereklidir" } },
                        },
                        lastName: {
                            validators: { notEmpty: { message: "Soyadı gereklidir" } },
                        },
                        userName: {
                            validators: { notEmpty: { message: "Kullanıcı adı gereklidir" } },
                        },
                        email: {
                            validators: {
                                notEmpty: { message: "Geçerli bir e-posta adresi gereklidir" },
                                emailAddress: { message: "Lütfen geçerli bir e-posta adresi girin" },
                            },
                        },
                      
                    },
                    plugins: {
                        trigger: new FormValidation.plugins.Trigger(),
                        bootstrap: new FormValidation.plugins.Bootstrap5({
                            rowSelector: ".fv-row",
                            eleInvalidClass: "",
                            eleValidClass: "",
                        }),
                    },
                });

                const submitButton = modal.querySelector('[data-kt-users-modal-action="submit"]');
                submitButton.addEventListener("click", (e) => {
                    e.preventDefault();
                    if (formValidator) {
                        formValidator.validate().then(function (status) {
                            if (status === "Valid") {
                                submitButton.setAttribute("data-kt-indicator", "on");
                                submitButton.disabled = true;

                                const formData = new FormData(form);

                                $.ajax({
                                    type: 'POST',
                                    url: '/User/CreateUser',
                                    data: formData,
                                    processData: false,
                                    contentType: false,
                                    success: function (response) {
                                        submitButton.removeAttribute("data-kt-indicator");
                                        submitButton.disabled = false;
                                        if (response.success) {
                                            Swal.fire({
                                                text: response.message,
                                                icon: "success",
                                                buttonsStyling: false,
                                                confirmButtonText: "Tamam",
                                                customClass: { confirmButton: "btn btn-primary" },
                                            }).then(function (result) {
                                                if (result.isConfirmed) {
                                                    modalInstance.hide();
                                                    location.reload(); // Refresh or update the list
                                                }
                                            });
                                        } else {
                                            Swal.fire({
                                                text: response.message,
                                                icon: "error",
                                                buttonsStyling: false,
                                                confirmButtonText: "Tamam",
                                                customClass: { confirmButton: "btn btn-primary" },
                                            });
                                        }
                                    },
                                    error: function (xhr, status, error) {
                                        submitButton.removeAttribute("data-kt-indicator");
                                        submitButton.disabled = false;
                                        Swal.fire({
                                            text: "Kullanıcı oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.",
                                            icon: "error",
                                            buttonsStyling: false,
                                            confirmButtonText: "Tamam",
                                            customClass: { confirmButton: "btn btn-primary" },
                                        });
                                    }
                                });
                            } else {
                                Swal.fire({
                                    text: "Üzgünüz, bazı hatalar tespit edildi, lütfen tekrar deneyin.",
                                    icon: "error",
                                    buttonsStyling: false,
                                    confirmButtonText: "Tamam",
                                    customClass: { confirmButton: "btn btn-primary" },
                                });
                            }
                        });
                    }
                });

                // Cancel button handler
                modal.querySelector('[data-kt-users-modal-action="cancel"]').addEventListener("click", (e) => {
                    e.preventDefault();
                    Swal.fire({
                        text: "İptal etmek istediğinizden emin misiniz?",
                        icon: "warning",
                        showCancelButton: true,
                        buttonsStyling: false,
                        confirmButtonText: "Evet, iptal et!",
                        cancelButtonText: "Hayır, geri dön",
                        customClass: {
                            confirmButton: "btn btn-primary",
                            cancelButton: "btn btn-active-light",
                        },
                    }).then(function (result) {
                        if (result.value) {
                            form.reset();
                            modalInstance.hide();
                        } else if (result.dismiss === Swal.DismissReason.cancel) {
                            Swal.fire({
                                text: "Formunuz iptal edilmedi!",
                                icon: "error",
                                buttonsStyling: false,
                                confirmButtonText: "Tamam",
                                customClass: { confirmButton: "btn btn-primary" },
                            });
                        }
                    });
                });

                // Close button handler
                modal.querySelector('[data-kt-users-modal-action="close"]').addEventListener("click", (e) => {
                    e.preventDefault();
                    Swal.fire({
                        text: "İptal etmek istediğinizden emin misiniz?",
                        icon: "warning",
                        showCancelButton: true,
                        buttonsStyling: false,
                        confirmButtonText: "Evet, iptal et!",
                        cancelButtonText: "Hayır, geri dön",
                        customClass: {
                            confirmButton: "btn btn-primary",
                            cancelButton: "btn btn-active-light",
                        },
                    }).then(function (result) {
                        if (result.value) {
                            form.reset();
                            modalInstance.hide();
                        } else if (result.dismiss === Swal.DismissReason.cancel) {
                            Swal.fire({
                                text: "Formunuz iptal edilmedi!",
                                icon: "error",
                                buttonsStyling: false,
                                confirmButtonText: "Tamam",
                                customClass: { confirmButton: "btn btn-primary" },
                            });
                        }
                    });
                });
            })();
        },
    };
})();

KTUtil.onDOMContentLoaded(function () {
    KTUsersAddUser.init();
});