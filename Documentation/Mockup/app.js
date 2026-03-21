document.addEventListener('DOMContentLoaded', () => {
    // ----------------------------------------------------------------------
    // 1. Mock Daten (Mindestens 10 Übungen)
    // ----------------------------------------------------------------------
    const mockExercises = [
        { id: 1, name: "Bankdrücken", muscle: ["Brust", "Trizeps", "Schulter"], image: "https://placehold.co/100x100/3b82f6/white?text=BD" },
        { id: 2, name: "Kniebeugen", muscle: ["Beine", "Po", "Core"], image: "https://placehold.co/100x100/10b981/white?text=KB" },
        { id: 3, name: "Kreuzheben", muscle: ["Rücken", "Beine", "Core"], image: "https://placehold.co/100x100/f59e0b/white?text=KH" },
        { id: 4, name: "Klimmzüge", muscle: ["Rücken", "Bizeps"], image: "https://placehold.co/100x100/ef4444/white?text=Klimm" },
        { id: 5, name: "Schulterdrücken", muscle: ["Schultern", "Trizeps"], image: "https://placehold.co/100x100/8b5cf6/white?text=SD" },
        { id: 6, name: "Langhantel-Rudern", muscle: ["Rücken", "Bizeps"], image: "https://placehold.co/100x100/ec4899/white?text=Rud" },
        { id: 7, name: "Beinpresse", muscle: ["Beine", "Waden"], image: "https://placehold.co/100x100/06b6d4/white?text=BP" },
        { id: 8, name: "Bizeps Curls", muscle: ["Bizeps"], image: "https://placehold.co/100x100/f97316/white?text=BC" },
        { id: 9, name: "Trizeps Drücken (Kabel)", muscle: ["Trizeps"], image: "https://placehold.co/100x100/64748b/white?text=TD" },
        { id: 10, name: "Wadenheben stehend", muscle: ["Waden"], image: "https://placehold.co/100x100/14b8a6/white?text=WH" },
        { id: 11, name: "French Press", muscle: ["Trizeps"], image: "https://placehold.co/100x100/f43f5e/white?text=FP" },
        { id: 12, name: "Crunches", muscle: ["Bauch"], image: "https://placehold.co/100x100/84cc16/white?text=Cr" }
    ];

    let selectedExercises = [];

    // ----------------------------------------------------------------------
    // 2. Choices.js Initialisierung (Analog zu ASP.NET MVC Schema)
    // ----------------------------------------------------------------------
    const exerciseSelectEl = document.getElementById('exerciseSelect');

    const choices = new Choices(exerciseSelectEl, {
        searchEnabled: true,
        itemSelectText: '',
        placeholder: true,
        placeholderValue: 'Wähle eine Übung...',
        searchPlaceholderValue: 'Suchen...',
        noResultsText: 'Keine Übung gefunden',
        shouldSort: false,
        allowHTML: true,
        callbackOnCreateTemplates: function(template) {
            return {
                choice: (classNames, data) => {
                    // Custom HTML Renderer für das Dropdown-Menü
                    const muscles = data.customProperties && data.customProperties.muscles ? data.customProperties.muscles : [];
                    const tags = muscles.map(m => `<span class="badge bg-secondary me-1" style="font-size: 0.7rem;">${m}</span>`).join('');
                    const imageSrc = data.customProperties && data.customProperties.image ? data.customProperties.image : '';
                    
                    return template(`
                        <div class="${classNames.item} ${classNames.itemChoice} ${data.disabled ? classNames.itemDisabled : classNames.itemSelectable}" data-select-text="${this.config.itemSelectText}" data-choice ${data.disabled ? 'data-choice-disabled aria-disabled="true"' : 'data-choice-selectable'} data-id="${data.id}" data-value="${data.value}" ${data.groupId > 0 ? 'role="treeitem"' : 'role="option"'}>
                            <div class="d-flex align-items-center">
                                <img src="${imageSrc}" class="rounded me-3" style="width: 50px; height: 50px; object-fit: cover;">
                                <div>
                                    <div class="fw-bold">${data.label}</div>
                                    <div class="mt-1">${tags}</div>
                                </div>
                            </div>
                        </div>
                    `);
                }
            };
        }
    });

    // Populate choices from mock data
    const choicesData = mockExercises.map(ex => ({
        value: ex.id,
        label: ex.name,
        customProperties: {
            image: ex.image,
            muscles: Array.isArray(ex.muscle) ? ex.muscle : [ex.muscle]
        }
    }));
    choices.setChoices(choicesData, 'value', 'label', true);

    // Event Listener für Auswahl in Choices
    exerciseSelectEl.addEventListener('change', function(e) {
        const selectedId = parseInt(e.target.value);
        if (selectedId) {
            const exercise = mockExercises.find(x => x.id === selectedId);
            if (exercise) {
                addExerciseToPlan(exercise);
            }
            // Wert nach Auswahl wieder zurücksetzen, damit Dropdown leer erscheint
            choices.setChoiceByValue('');
        }
    });

    // ----------------------------------------------------------------------
    // 3. UI Elemente & SortableJS
    // ----------------------------------------------------------------------
    const selectedExercisesList = document.getElementById('selectedExercisesList');
    const emptyState = document.getElementById('emptyState');
    const badge = document.getElementById('exerciseCountBadge');
    
    // SortableJS Initialisierung für Drag & Drop (optimiert für Mobile 1-zu-1 Tracking)
    new Sortable(selectedExercisesList, {
        animation: 150,
        handle: '.drag-handle',
        ghostClass: 'sortable-ghost',
        dragClass: 'sortable-drag',
        fallbackClass: 'sortable-fallback',
        forceFallback: true,      // Erlaubt das Styling des schwebenden Elements, unabhängig vom OS
        fallbackTolerance: 3,     // Verhindert versehentliches Ziehen bei Klicks (3px Toleranz)
        onEnd: function() {
            syncArrayWithDOM();
        }
    });

    function addExerciseToPlan(exercise) {
        // Generiere eine eindeutige ID (falls gleiche Übung mehrmals hinzugefügt wird)
        const instanceId = Date.now() + Math.random().toString().substr(2, 5);
        
        const exerciseItem = {
            instanceId: instanceId,
            id: exercise.id,
            name: exercise.name,
            image: exercise.image,
            muscle: Array.isArray(exercise.muscle) ? exercise.muscle : [exercise.muscle],
            sets: 3, // Default Werte
            reps: 10,
            poActive: false,
            poTargetMinReps: 8,
            poTargetMaxReps: 12,
            poWeightIncrement: 2.5,
            poRepsIncrement: 1
        };

        selectedExercises.push(exerciseItem);
        renderList();
    }

    function removeExercise(instanceId) {
        selectedExercises = selectedExercises.filter(ex => ex.instanceId !== instanceId);
        renderList();
    }

    window.removeExerciseGlobal = removeExercise; // Für Inline-Buttons

    function renderList() {
        selectedExercisesList.innerHTML = '';
        
        if (selectedExercises.length === 0) {
            emptyState.classList.remove('d-none');
        } else {
            emptyState.classList.add('d-none');
            
            selectedExercises.forEach((ex, index) => {
                const tagsStr = ex.muscle.map(m => `<span class="badge bg-secondary me-1" style="font-size: 0.7rem;">${m}</span>`).join('');
                const itemHtml = `
                    <div class="swipe-wrapper" data-wrapper-id="${ex.instanceId}">
                        <button type="button" class="swipe-action-btn" onclick="removeExerciseGlobal('${ex.instanceId}')" aria-label="Entfernen">
                            <i class="bi bi-trash3 fs-5 mb-1"></i>
                            Entfernen
                        </button>
                        <div class="exercise-item p-3" data-instance-id="${ex.instanceId}">
                            <div class="d-flex align-items-center mb-3 position-relative">
                                <i class="bi bi-grip-vertical drag-handle fs-4"></i>
                                <img src="${ex.image}" alt="${ex.name}" class="rounded ms-2 me-3" style="width: 65px; height: 65px; object-fit: cover;">
                                <div class="flex-grow-1">
                                    <div class="exercise-title">${index + 1}. ${ex.name}</div>
                                    <div class="mt-1">${tagsStr}</div>
                                </div>
                            </div>
                            <div class="row px-4">
                            <div class="col-6">
                                <div class="number-input-group">
                                    <label class="fw-bold">Sätze</label>
                                    <input type="number" class="form-control text-center input-sets" 
                                        min="1" max="20" value="${ex.sets}" 
                                        data-instance-id="${ex.instanceId}" required>
                                    <div class="invalid-feedback">Mindestens 1</div>
                                </div>
                            </div>
                            <div class="col-6">
                                <div class="number-input-group">
                                    <label class="fw-bold">Reps</label>
                                    <input type="number" class="form-control text-center input-reps" 
                                        min="1" max="100" value="${ex.reps}" 
                                        data-instance-id="${ex.instanceId}" required>
                                    <div class="invalid-feedback">Mindestens 1</div>
                                </div>
                            </div>
                            <!-- Progressive Overload Accordion -->
                            <div class="po-section mt-4 pt-3 border-top">
                                <div class="d-flex justify-content-start mb-2">
                                    <div class="form-check form-switch form-check-switch-lg d-flex align-items-center">
                                        <input class="form-check-input po-toggle m-0 flex-shrink-0" type="checkbox" role="switch" id="poToggle-${ex.instanceId}" data-instance-id="${ex.instanceId}" ${ex.poActive ? 'checked' : ''}>
                                        <label class="form-check-label ms-2 text-muted" style="font-size: 0.85rem;" for="poToggle-${ex.instanceId}">Progressive Overload</label>
                                    </div>
                                </div>
                                <div class="collapse po-collapse ${ex.poActive ? 'show' : ''}" id="poCollapse-${ex.instanceId}">
                                    <div class="row g-2 pt-2">
                                        <div class="col-6">
                                            <div class="number-input-group">
                                                <label class="text-muted" style="font-size: 0.75rem;">Ziel Min. Reps</label>
                                                <input type="number" class="form-control form-control-sm text-center input-po" data-field="poTargetMinReps" min="1" value="${ex.poTargetMinReps}" data-instance-id="${ex.instanceId}">
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="number-input-group">
                                                <label class="text-muted" style="font-size: 0.75rem;">Ziel Max. Reps</label>
                                                <input type="number" class="form-control form-control-sm text-center input-po" data-field="poTargetMaxReps" min="1" value="${ex.poTargetMaxReps}" data-instance-id="${ex.instanceId}">
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="number-input-group">
                                                <label class="text-muted" style="font-size: 0.75rem;">Gewicht + (kg)</label>
                                                <input type="number" step="0.5" class="form-control form-control-sm text-center input-po" data-field="poWeightIncrement" min="0" value="${ex.poWeightIncrement}" data-instance-id="${ex.instanceId}">
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="number-input-group">
                                                <label class="text-muted" style="font-size: 0.75rem;">Reps +</label>
                                                <input type="number" class="form-control form-control-sm text-center input-po" data-field="poRepsIncrement" min="0" value="${ex.poRepsIncrement}" data-instance-id="${ex.instanceId}">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-text mt-2" style="font-size: 0.75rem;">
                                        <i class="bi bi-info-circle me-1"></i>Wenn Max. Reps erreicht sind, wird im nächsten Training erhöht.
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                `;
                selectedExercisesList.insertAdjacentHTML('beforeend', itemHtml);
            });
            
            // Event Listener für Input Änderungen & Swipes
            attachInputListeners();
            attachSwipeListeners();
        }
        
        badge.textContent = selectedExercises.length;
    }

    function attachSwipeListeners() {
        document.querySelectorAll('.exercise-item').forEach(item => {
            let startX = 0;
            let currentX = 0;
            let isDraggingSwipe = false;

            item.addEventListener('touchstart', (e) => {
                // Ignore if it's the drag handle for Sortable JS
                if (e.target.closest('.drag-handle') || e.target.closest('input')) return;
                
                startX = e.touches[0].clientX;
                isDraggingSwipe = true;
                item.style.transition = 'none';
            }, {passive: true});

            item.addEventListener('touchmove', (e) => {
                if (!isDraggingSwipe) return;
                currentX = e.touches[0].clientX;
                const diffX = currentX - startX;
                
                // Allow only left swipe between 0 and -90px limit
                if (diffX < 0) {
                    const translateX = Math.max(diffX, -90);
                    item.style.transform = `translateX(${translateX}px)`;
                } else {
                    item.style.transform = `translateX(0px)`;
                }
            }, {passive: true});

            item.addEventListener('touchend', (e) => {
                if (!isDraggingSwipe) return;
                isDraggingSwipe = false;
                item.style.transition = 'transform 0.3s ease-out';
                
                const diffX = currentX - startX;
                // Snap threshold: 45px
                if (diffX < -45) {
                    item.style.transform = `translateX(-80px)`; // Show exactly the 80px button
                } else {
                    item.style.transform = `translateX(0px)`;
                }
            });
        });
    }

    function attachInputListeners() {
        document.querySelectorAll('.input-sets').forEach(input => {
            input.addEventListener('change', (e) => {
                const id = e.target.getAttribute('data-instance-id');
                const val = parseInt(e.target.value);
                const ex = selectedExercises.find(x => x.instanceId === id);
                if(ex) ex.sets = val;
            });
        });

        document.querySelectorAll('.input-reps').forEach(input => {
            input.addEventListener('change', (e) => {
                const id = e.target.getAttribute('data-instance-id');
                const val = parseInt(e.target.value);
                const ex = selectedExercises.find(x => x.instanceId === id);
                if(ex) ex.reps = val;
            });
        });

        document.querySelectorAll('.po-toggle').forEach(toggle => {
            toggle.addEventListener('change', (e) => {
                const id = e.target.getAttribute('data-instance-id');
                const ex = selectedExercises.find(x => x.instanceId === id);
                if(ex) ex.poActive = e.target.checked;
                
                const collapseEl = document.getElementById(`poCollapse-${id}`);
                const bsCollapse = bootstrap.Collapse.getOrCreateInstance(collapseEl, { toggle: false });
                if (e.target.checked) {
                    bsCollapse.show();
                } else {
                    bsCollapse.hide();
                }
            });
        });

        document.querySelectorAll('.input-po').forEach(input => {
            input.addEventListener('change', (e) => {
                const id = e.target.getAttribute('data-instance-id');
                const field = e.target.getAttribute('data-field');
                const val = parseFloat(e.target.value);
                const ex = selectedExercises.find(x => x.instanceId === id);
                if(ex) ex[field] = val;
            });
        });
    }

    function syncArrayWithDOM() {
        const domItems = document.querySelectorAll('.swipe-wrapper');
        const newArray = [];
        domItems.forEach((item, index) => {
            const id = item.getAttribute('data-wrapper-id');
            const exObj = selectedExercises.find(ex => ex.instanceId === id);
            if(exObj) {
                newArray.push(exObj);
                // Nummerierung in Text aktualisieren
                const titleEl = item.querySelector('.exercise-title');
                if(titleEl) {
                    titleEl.textContent = `${index + 1}. ${exObj.name}`;
                }
            }
        });
        selectedExercises = newArray;
    }

    // ----------------------------------------------------------------------
    // 4. Formular Validierung & Submit (Fehlerbehandlung)
    // ----------------------------------------------------------------------
    const form = document.getElementById('trainingPlanForm');
    const planNameInput = document.getElementById('planName');
    const globalError = document.getElementById('globalError');

    // Entferne Error on Typing
    planNameInput.addEventListener('input', () => {
        if(planNameInput.value.trim().length > 0) {
            planNameInput.classList.remove('is-invalid');
            globalError.classList.add('d-none');
        }
    });

    form.addEventListener('submit', function(e) {
        e.preventDefault();
        e.stopPropagation();
        
        const isValid = validatePlan();
        
        if (isValid) {
            // Erfolgreiches Speichern simulieren
            const saveBtn = document.getElementById('savePlanBtn');
            const originalText = saveBtn.innerHTML;
            saveBtn.innerHTML = '<i class="bi bi-check-circle-fill me-2"></i>Gespeichert!';
            saveBtn.classList.replace('btn-primary', 'btn-success');
            
            setTimeout(() => {
                alert('Mockup: Plan wurde erfolgreich validiert und gespeichert!');
                saveBtn.innerHTML = originalText;
                saveBtn.classList.replace('btn-success', 'btn-primary');
                // Reset form optionally
            }, 1000);
        }
    });

    function validatePlan(isSoft = false) {
        let isValid = true;
        let errorMsg = [];

        // 1. Plan Name
        if (planNameInput.value.trim() === '') {
            planNameInput.classList.add('is-invalid');
            isValid = false;
            errorMsg.push('Der Trainingsplan muss einen Namen haben.');
        } else {
            planNameInput.classList.remove('is-invalid');
        }

        // 2. Anzahl Übungen
        if (selectedExercises.length === 0) {
            isValid = false;
            errorMsg.push('Füge mindestens eine Übung zum Plan hinzu.');
        }

        // 3. Sätze & Reps Check & PO Check
        let inputsValid = true;
        document.querySelectorAll('.input-sets, .input-reps').forEach(input => {
            const val = parseInt(input.value);
            if (isNaN(val) || val <= 0) {
                input.classList.add('is-invalid');
                inputsValid = false;
            } else {
                input.classList.remove('is-invalid');
            }
        });

        document.querySelectorAll('.input-po').forEach(input => {
            const id = input.getAttribute('data-instance-id');
            const ex = selectedExercises.find(x => x.instanceId === id);
            // Nur validieren, wenn Progressive Overload aktiv ist
            if (ex && ex.poActive) {
                const val = parseFloat(input.value);
                if (isNaN(val) || val < 0) {
                    input.classList.add('is-invalid');
                    inputsValid = false;
                } else {
                    input.classList.remove('is-invalid');
                }
            } else {
                // Bei nicht aktivem PO keine Invalidierung ankreiden
                input.classList.remove('is-invalid');
            }
        });

        if (!inputsValid) {
            isValid = false;
            errorMsg.push('Zahlenwerte müssen gültig und größer als 0 sein (Gilt auch für aktives PO).');
        }

        if (!isValid && !isSoft) {
            globalError.innerHTML = '<strong>Achtung:</strong><br>' + errorMsg.join('<br>');
            globalError.classList.remove('d-none');
            
            // Scrolle hoch, falls der Fehler oben ist, aber auf Handys
            if(errorMsg.includes('Der Trainingsplan muss einen Namen haben.')){
                 window.scrollTo({ top: 0, behavior: 'smooth' });
            }
        } else if (isValid) {
            globalError.classList.add('d-none');
        }

        return isValid;
    }

    // ----------------------------------------------------------------------
    // 5. Darkmode Toggle
    // ----------------------------------------------------------------------
    const themeToggle = document.getElementById('themeToggle');
    const themeIcon = document.getElementById('themeIcon');
    const htmlEl = document.documentElement;
    
    // Check local storage or system preference
    const currentTheme = localStorage.getItem('theme') || (window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light');
    
    function setTheme(theme) {
        htmlEl.setAttribute('data-theme', theme);
        localStorage.setItem('theme', theme);
        
        if (theme === 'dark') {
            themeIcon.classList.replace('bi-moon-fill', 'bi-sun-fill');
            themeIcon.classList.add('text-warning');
        } else {
            themeIcon.classList.replace('bi-sun-fill', 'bi-moon-fill');
            themeIcon.classList.remove('text-warning');
        }
    }

    setTheme(currentTheme);

    themeToggle.addEventListener('click', () => {
        const newTheme = htmlEl.getAttribute('data-theme') === 'dark' ? 'light' : 'dark';
        setTheme(newTheme);
    });
});
