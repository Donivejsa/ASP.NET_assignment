// Client-side validation for project forms
document.addEventListener('DOMContentLoaded', function () {
    // Add Project form validation
    const addProjectForm = document.getElementById('addProjectForm');
    if (addProjectForm) {
        addProjectForm.addEventListener('submit', function (event) {
            let isValid = true;

            // Get form fields
            const nameInput = document.getElementById('ProjectInput_Name');
            const descriptionInput = document.getElementById('ProjectInput_Description');
            const startDateInput = document.getElementById('ProjectInput_StartDate');
            const endDateInput = document.getElementById('ProjectInput_EndDate');

            // Validate project name
            if (!nameInput.value.trim()) {
                isValid = false;
                showError(nameInput, 'Project name is required');
            } else if (nameInput.value.length > 100) {
                isValid = false;
                showError(nameInput, 'Project name cannot exceed 100 characters');
            } else {
                clearError(nameInput);
            }

            // Validate description
            if (!descriptionInput.value.trim()) {
                isValid = false;
                showError(descriptionInput, 'Description is required');
            } else {
                clearError(descriptionInput);
            }

            // Validate start date
            if (!startDateInput.value) {
                isValid = false;
                showError(startDateInput, 'Start date is required');
            } else {
                clearError(startDateInput);
            }

            // Validate end date if provided
            if (endDateInput.value && startDateInput.value) {
                const startDate = new Date(startDateInput.value);
                const endDate = new Date(endDateInput.value);

                if (endDate < startDate) {
                    isValid = false;
                    showError(endDateInput, 'End date cannot be earlier than start date');
                } else {
                    clearError(endDateInput);
                }
            }

            if (!isValid) {
                event.preventDefault();
            }
        });
    }

    // Edit Project form validation
    const editProjectForm = document.getElementById('editProjectForm');
    if (editProjectForm) {
        editProjectForm.addEventListener('submit', function (event) {
            let isValid = true;

            // Get form fields
            const nameInput = document.getElementById('editProjectName');
            const descriptionInput = document.getElementById('editProjectDescription');
            const startDateInput = document.getElementById('editStartDate');
            const endDateInput = document.getElementById('editEndDate');

            // Validate project name
            if (!nameInput.value.trim()) {
                isValid = false;
                showError(nameInput, 'Project name is required');
            } else if (nameInput.value.length > 100) {
                isValid = false;
                showError(nameInput, 'Project name cannot exceed 100 characters');
            } else {
                clearError(nameInput);
            }

            // Validate description
            if (!descriptionInput.value.trim()) {
                isValid = false;
                showError(descriptionInput, 'Description is required');
            } else {
                clearError(descriptionInput);
            }

            // Validate start date
            if (!startDateInput.value) {
                isValid = false;
                showError(startDateInput, 'Start date is required');
            } else {
                clearError(startDateInput);
            }

            // Validate end date if provided
            if (endDateInput.value && startDateInput.value) {
                const startDate = new Date(startDateInput.value);
                const endDate = new Date(endDateInput.value);

                if (endDate < startDate) {
                    isValid = false;
                    showError(endDateInput, 'End date cannot be earlier than start date');
                } else {
                    clearError(endDateInput);
                }
            }

            if (!isValid) {
                event.preventDefault();
            }
        });
    }

    // Helper function to show error message
    function showError(input, message) {
        // Clear any existing error
        clearError(input);

        // Create error element
        const errorElement = document.createElement('div');
        errorElement.className = 'text-danger';
        errorElement.textContent = message;

        // Insert error after input
        input.parentNode.insertBefore(errorElement, input.nextSibling);

        // Add error class to input
        input.classList.add('input-error');
    }

    // Helper function to clear error message
    function clearError(input) {
        // Remove error message if exists
        const parent = input.parentNode;
        const errorElement = parent.querySelector('.text-danger');
        if (errorElement) {
            parent.removeChild(errorElement);
        }

        // Remove error class from input
        input.classList.remove('input-error');
    }

    // Close dropdown menus when clicking outside
    document.addEventListener('click', function (event) {
        const actionMenus = document.querySelectorAll('.project-actions-menu.show');
        actionMenus.forEach(menu => {
            const button = menu.previousElementSibling;
            if (!menu.contains(event.target) && !button.contains(event.target)) {
                menu.classList.remove('show');
            }
        });
    });
});