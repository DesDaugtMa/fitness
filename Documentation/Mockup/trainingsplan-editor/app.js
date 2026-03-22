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

    // ----------------------------------------------------------------------
    // 2. State Management (LocalStorage)
    // ----------------------------------------------------------------------
    const STORAGE_KEY_DAYS = 'tpMockup_days';
    const STORAGE_KEY_NAME = 'tpMockup_planName';
    const STORAGE_KEY_DESC = 'tpMockup_planDesc';

    function saveState() {
        localStorage.setItem(STORAGE_KEY_NAME, document.getElementById('planName').value);
        localStorage.setItem(STORAGE_KEY_DESC, document.getElementById('planDesc').value || '');
        localStorage.setItem(STORAGE_KEY_DAYS, JSON.stringify(trainingDays));
    }

    function loadState() {
        const savedName = localStorage.getItem(STORAGE_KEY_NAME);
        if (savedName) document.getElementById('planName').value = savedName;

        const savedDesc = localStorage.getItem(STORAGE_KEY_DESC);
        if (savedDesc) document.getElementById('planDesc').value = savedDesc;

        const savedDays = localStorage.getItem(STORAGE_KEY_DAYS);
        if (savedDays) {
            try {
                const parsed = JSON.parse(savedDays);
                if (Array.isArray(parsed) && parsed.length === 7) {
                    return parsed;
                }
            } catch (e) {
                console.error("Localstorage konnte nicht geladen werden:", e);
            }
        }
        
        // Fallback: 7 Standardtage wenn noch nie gespeichert
        return ["Montag", "Dienstag", "Mittwoch", "Donnerstag", "Freitag", "Samstag", "Sonntag"].map(name => ({
            id: generateId(),
            name: name,
            isRestDay: false,
            exercises: []
        }));
    }

    // ----------------------------------------------------------------------
    // 3. Day & Exercise Management
    // ----------------------------------------------------------------------
    let trainingDays = [];
    
    const daysContainer = document.getElementById('daysContainer');
    const dayTemplate = document.getElementById('dayTemplate');
    const globalError = document.getElementById('globalError');

    // Mache remove global verfügbar
    window.removeExerciseGlobal = function(dayId, instanceId) {
        const day = trainingDays.find(d => d.id === dayId);
        if(day) {
            day.exercises = day.exercises.filter(ex => ex.instanceId !== instanceId);
            renderDayExercises(dayId);
            saveState(); // Speichern nach dem Löschen einer Übung
        }
    };

    function generateId() {
        return 'id-' + Date.now() + Math.random().toString(36).substr(2, 5);
    }

    function initDays() {
        const tabsContainer = document.getElementById('dayTabsContainer');
        tabsContainer.innerHTML = '';
        daysContainer.innerHTML = '';
        
        trainingDays = loadState();
        
        trainingDays.forEach((loadedDay, index) => {
            // --- Tab Erstellen ---
            const tabLi = document.createElement('li');
            tabLi.className = 'nav-item';
            const tabBtn = document.createElement('button');
            tabBtn.className = `nav-link shadow-sm ${index === 0 ? 'active' : ''}`;
            tabBtn.type = 'button';
            tabBtn.innerText = loadedDay.name.trim() !== '' ? loadedDay.name : '(Unbenannt)';
            tabBtn.setAttribute('data-target-day', loadedDay.id);
            tabLi.appendChild(tabBtn);
            tabsContainer.appendChild(tabLi);

            // --- Day Card Templating ---
            const clone = dayTemplate.content.cloneNode(true);
            const dayCard = clone.querySelector('.day-card');
            dayCard.setAttribute('data-day-id', loadedDay.id);
            if (index !== 0) {
                dayCard.classList.add('d-none'); // Nur erster Tag ist am Anfang sichtbar
            }
            
            // Tab Klick Verhalten
            tabBtn.addEventListener('click', () => {
                tabsContainer.querySelectorAll('.nav-link').forEach(btn => btn.classList.remove('active'));
                tabBtn.classList.add('active');
                
                document.querySelectorAll('.day-card').forEach(card => card.classList.add('d-none'));
                const targetCard = document.querySelector(`.day-card[data-day-id="${loadedDay.id}"]`);
                if(targetCard) targetCard.classList.remove('d-none');
            });

            // Name Input
            const nameInput = dayCard.querySelector('.day-name-input');
            nameInput.value = loadedDay.name;
            
            nameInput.addEventListener('input', (e) => {
                loadedDay.name = e.target.value;
                nameInput.classList.remove('is-invalid');
                // Synchronisiere Tab Name
                tabBtn.innerText = loadedDay.name.trim() !== '' ? loadedDay.name : '(Unbenannt)';
                saveState(); // Speichern bei Tippen im Namen
            });

            // Restday Toggle
            const restdayToggle = dayCard.querySelector('.restday-toggle');
            const dayContent = dayCard.querySelector('.day-content');
            
            restdayToggle.checked = loadedDay.isRestDay;
            if(loadedDay.isRestDay) {
                dayContent.classList.add('d-none');
            }
            
            restdayToggle.addEventListener('change', (e) => {
                loadedDay.isRestDay = e.target.checked;
                if(loadedDay.isRestDay) {
                    dayContent.classList.add('d-none');
                    // Kein opacity styling mehr
                    dayCard.querySelectorAll('.is-invalid').forEach(el => el.classList.remove('is-invalid'));
                } else {
                    dayContent.classList.remove('d-none');
                }
                saveState(); // Speichern bei Toggle
            });

            // Initialize Choices.js
            const selectEl = dayCard.querySelector('.day-exercise-select');
            const choices = new Choices(selectEl, {
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

            const choicesData = mockExercises.map(ex => ({
                value: ex.id,
                label: ex.name,
                customProperties: {
                    image: ex.image,
                    muscles: Array.isArray(ex.muscle) ? ex.muscle : [ex.muscle]
                }
            }));
            choices.setChoices(choicesData, 'value', 'label', true);

            selectEl.addEventListener('change', function(e) {
                const selectedId = parseInt(e.target.value);
                if (selectedId) {
                    const exercise = mockExercises.find(x => x.id === selectedId);
                    if (exercise) {
                        addExerciseToDay(loadedDay.id, exercise);
                    }
                    choices.setChoiceByValue('');
                }
            });

            // Initialize Sortable
            const listContainer = dayCard.querySelector('.day-exercises-list');
            new Sortable(listContainer, {
                animation: 150,
                handle: '.drag-handle',
                ghostClass: 'sortable-ghost',
                dragClass: 'sortable-drag',
                fallbackClass: 'sortable-fallback',
                forceFallback: true,      
                fallbackTolerance: 3,     
                group: 'shared', // Erlaubt verschieben zwischen Tagen
                onEnd: function() {
                    syncAllArraysWithDOM(); // Ruft auch saveState intern auf
                }
            });

            daysContainer.appendChild(dayCard);
            renderDayExercises(loadedDay.id);
        });
    }

    function addExerciseToDay(dayId, exercise) {
        const day = trainingDays.find(d => d.id === dayId);
        if (!day) return;

        const instanceId = generateId();
        
        const exerciseItem = {
            instanceId: instanceId,
            id: exercise.id,
            name: exercise.name,
            image: exercise.image,
            muscle: Array.isArray(exercise.muscle) ? exercise.muscle : [exercise.muscle],
            sets: 3,
            reps: 10,
            poActive: false,
            poTargetMinReps: 8,
            poTargetMaxReps: 12,
            poWeightIncrement: 2.5,
            poRepsIncrement: 1
        };

        day.exercises.push(exerciseItem);
        renderDayExercises(dayId);
        saveState(); // Speichern!
    }

    function findExerciseByInstanceId(instanceId) {
        for(let d of trainingDays) {
            const ex = d.exercises.find(x => x.instanceId === instanceId);
            if(ex) return ex;
        }
        return null;
    }

    function syncAllArraysWithDOM() {
        const dayCards = document.querySelectorAll('.day-card');
        dayCards.forEach(card => {
            const dayId = card.getAttribute('data-day-id');
            const day = trainingDays.find(d => d.id === dayId);
            if(day) {
                const newArray = [];
                const domItems = card.querySelectorAll('.swipe-wrapper');
                domItems.forEach((item, index) => {
                    const instId = item.getAttribute('data-wrapper-id');
                    const exObj = findExerciseByInstanceId(instId);
                    if(exObj) {
                        newArray.push(exObj);
                        const titleEl = item.querySelector('.exercise-title');
                        if(titleEl) {
                            titleEl.textContent = `${index + 1}. ${exObj.name}`;
                        }
                    }
                });
                day.exercises = newArray;
            }
        });
        
        // Update Empty States
        trainingDays.forEach(d => {
            const card = document.querySelector(`.day-card[data-day-id="${d.id}"]`);
            if(card) {
                const empty = card.querySelector('.day-empty-state');
                if(d.exercises.length === 0) {
                    empty.classList.remove('d-none');
                } else {
                    empty.classList.add('d-none');
                }
            }
        });
        saveState(); // Immer speichern nach dem Umsortieren!
    }

    function renderDayExercises(dayId) {
        const dayCard = document.querySelector(`.day-card[data-day-id="${dayId}"]`);
        if(!dayCard) return;
        
        const list = dayCard.querySelector('.day-exercises-list');
        const emptyState = dayCard.querySelector('.day-empty-state');
        const day = trainingDays.find(d => d.id === dayId);
        
        list.innerHTML = '';
        
        if (day.exercises.length === 0) {
            emptyState.classList.remove('d-none');
        } else {
            emptyState.classList.add('d-none');
            
            day.exercises.forEach((ex, index) => {
                const tagsStr = ex.muscle.map(m => `<span class="badge bg-secondary me-1" style="font-size: 0.7rem;">${m}</span>`).join('');
                const itemHtml = `
                    <div class="swipe-wrapper" data-wrapper-id="${ex.instanceId}">
                        <button type="button" class="swipe-action-btn" onclick="removeExerciseGlobal('${dayId}', '${ex.instanceId}')" aria-label="Entfernen">
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
                            <div class="col-12 po-section mt-4 pt-3 border-top">
                                <div class="d-flex justify-content-start mb-2">
                                    <div class="form-check form-switch form-check-switch-lg d-flex align-items-center ps-0">
                                        <input class="form-check-input po-toggle m-0 flex-shrink-0 ms-0" type="checkbox" role="switch" id="poToggle-${ex.instanceId}" data-instance-id="${ex.instanceId}" ${ex.poActive ? 'checked' : ''}>
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
                    </div>
                `;
                list.insertAdjacentHTML('beforeend', itemHtml);
            });
            
            attachInputListeners(dayCard);
            attachSwipeListeners(dayCard);
        }
    }

    function attachSwipeListeners(container) {
        container.querySelectorAll('.exercise-item').forEach(item => {
            let startX = 0;
            let currentX = 0;
            let currentOffset = 0;
            let isDraggingSwipe = false;

            item.addEventListener('touchstart', (e) => {
                // Ignore interaction with forms / accordion
                if (e.target.closest('.drag-handle') || e.target.closest('input') || e.target.closest('.po-section')) return;
                
                startX = e.touches[0].clientX;
                isDraggingSwipe = true;
                
                // Ermittle aktuelle Position, damit man in beide Richtungen sanft swipen kann
                const match = item.style.transform.match(/translateX\(([-0-9.]+)px\)/);
                currentOffset = match ? parseFloat(match[1]) : 0;
                
                item.style.transition = 'none';
            }, {passive: true});

            item.addEventListener('touchmove', (e) => {
                if (!isDraggingSwipe) return;
                currentX = e.touches[0].clientX;
                const diffX = currentX - startX;
                
                // Addiere neue Bewegung zur bestehenden Position
                let newTranslate = currentOffset + diffX;
                
                // Limitiere den Swipe innerhalb von 0 und -90
                if (newTranslate > 0) newTranslate = 0;
                if (newTranslate < -90) newTranslate = -90;
                
                item.style.transform = `translateX(${newTranslate}px)`;
            }, {passive: true});

            item.addEventListener('touchend', (e) => {
                if (!isDraggingSwipe) return;
                isDraggingSwipe = false;
                item.style.transition = 'transform 0.3s ease-out';
                
                // Basierend auf dem Loslass-Punkt einrasten lassen
                const match = item.style.transform.match(/translateX\(([-0-9.]+)px\)/);
                const finalOffset = match ? parseFloat(match[1]) : 0;
                
                if (finalOffset < -40) {
                    item.style.transform = `translateX(-80px)`;
                } else {
                    item.style.transform = `translateX(0px)`;
                }
            });
        });
    }

    function attachInputListeners(container) {
        container.querySelectorAll('.input-sets').forEach(input => {
            input.addEventListener('change', (e) => {
                const id = e.target.getAttribute('data-instance-id');
                const ex = findExerciseByInstanceId(id);
                if(ex) ex.sets = parseInt(e.target.value);
                saveState(); // Speichern!
            });
        });

        container.querySelectorAll('.input-reps').forEach(input => {
            input.addEventListener('change', (e) => {
                const id = e.target.getAttribute('data-instance-id');
                const ex = findExerciseByInstanceId(id);
                if(ex) ex.reps = parseInt(e.target.value);
                saveState(); // Speichern!
            });
        });

        container.querySelectorAll('.po-toggle').forEach(toggle => {
            toggle.addEventListener('change', (e) => {
                const id = e.target.getAttribute('data-instance-id');
                const ex = findExerciseByInstanceId(id);
                if(ex) ex.poActive = e.target.checked;
                
                const collapseEl = document.getElementById(`poCollapse-${id}`);
                const bsCollapse = bootstrap.Collapse.getOrCreateInstance(collapseEl, { toggle: false });
                if (e.target.checked) {
                    bsCollapse.show();
                } else {
                    bsCollapse.hide();
                }
                saveState(); // Speichern!
            });
        });

        container.querySelectorAll('.input-po').forEach(input => {
            input.addEventListener('change', (e) => {
                const id = e.target.getAttribute('data-instance-id');
                const field = e.target.getAttribute('data-field');
                const ex = findExerciseByInstanceId(id);
                if(ex) ex[field] = parseFloat(e.target.value);
                saveState(); // Speichern!
            });
        });
    }

    // ----------------------------------------------------------------------
    // 4. Formular Validierung & Submit (Fehlerbehandlung)
    // ----------------------------------------------------------------------
    const form = document.getElementById('trainingPlanForm');
    const planNameInput = document.getElementById('planName');
    const planDescInput = document.getElementById('planDesc');

    // Listener für das Speichern von PlanName & PlanDesc
    planNameInput.addEventListener('input', () => {
        if(planNameInput.value.trim().length > 0) {
            planNameInput.classList.remove('is-invalid');
            globalError.classList.add('d-none');
        }
        saveState();
    });

    planDescInput.addEventListener('input', () => {
        saveState();
    });

    form.addEventListener('submit', function(e) {
        e.preventDefault();
        e.stopPropagation();
        
        const isValid = validatePlan();
        
        if (isValid) {
            // Lösche eventuell localStorage wenn man fertig ist?
            // localStorage.removeItem(STORAGE_KEY_DAYS);
            // localStorage.removeItem(STORAGE_KEY_NAME);
            // localStorage.removeItem(STORAGE_KEY_DESC);
            
            const saveBtn = document.getElementById('savePlanBtn');
            const originalText = saveBtn.innerHTML;
            saveBtn.innerHTML = '<i class="bi bi-check-circle-fill me-2"></i>Gespeichert!';
            saveBtn.classList.replace('btn-primary', 'btn-success');
            
            setTimeout(() => {
                alert('Mockup: Plan wurde erfolgreich validiert und gespeichert!');
                saveBtn.innerHTML = originalText;
                saveBtn.classList.replace('btn-success', 'btn-primary');
            }, 1000);
        }
    });

    function validatePlan(isSoft = false) {
        let isValid = true;
        let errorMsg = [];

        if (planNameInput.value.trim() === '') {
            planNameInput.classList.add('is-invalid');
            isValid = false;
            errorMsg.push('Der Trainingsplan muss einen Namen haben.');
        } else {
            planNameInput.classList.remove('is-invalid');
        }

        let emptyActiveDays = false;
        let unnamedDays = false;
        let allRestDays = true;
        
        trainingDays.forEach(day => {
            const nameInput = document.querySelector(`.day-card[data-day-id="${day.id}"] .day-name-input`);
            
            if (day.name.trim() === '') {
                unnamedDays = true;
                if(nameInput) nameInput.classList.add('is-invalid');
            } else {
                if(nameInput) nameInput.classList.remove('is-invalid');
            }

            if (!day.isRestDay) {
                allRestDays = false;
                if (day.exercises.length === 0) {
                    emptyActiveDays = true;
                }
            }
        });

        if (unnamedDays) {
            isValid = false;
            errorMsg.push('Jeder Tag muss einen Namen haben.');
        }

        if (allRestDays) {
            isValid = false;
            errorMsg.push('Ein Trainingsplan darf nicht nur aus Restdays bestehen!');
        } else if (emptyActiveDays) {
            isValid = false;
            errorMsg.push('Jeder Trainingstag (der kein Restday ist) muss mindestens eine Übung enthalten.');
        }

        let inputsValid = true;
        // Validate inputs only in active (non-restday) days
        trainingDays.filter(d => !d.isRestDay).forEach(day => {
            const dayCard = document.querySelector(`.day-card[data-day-id="${day.id}"]`);
            if(!dayCard) return;

            dayCard.querySelectorAll('.input-sets, .input-reps').forEach(input => {
                const val = parseInt(input.value);
                if (isNaN(val) || val <= 0) {
                    input.classList.add('is-invalid');
                    inputsValid = false;
                } else {
                    input.classList.remove('is-invalid');
                }
            });

            dayCard.querySelectorAll('.input-po').forEach(input => {
                const id = input.getAttribute('data-instance-id');
                const ex = findExerciseByInstanceId(id);
                if (ex && ex.poActive) {
                    const val = parseFloat(input.value);
                    if (isNaN(val) || val < 0) {
                        input.classList.add('is-invalid');
                        inputsValid = false;
                    } else {
                        input.classList.remove('is-invalid');
                    }
                } else {
                    input.classList.remove('is-invalid');
                }
            });
        });

        if (!inputsValid) {
            isValid = false;
            errorMsg.push('Zahlenwerte müssen gültig und größer als 0 sein (Gilt auch für aktives PO).');
        }

        if (!isValid && !isSoft) {
            globalError.innerHTML = '<strong>Achtung:</strong><br>' + errorMsg.join('<br>');
            globalError.classList.remove('d-none');
            
            if(errorMsg.includes('Der Trainingsplan muss einen Namen haben.')){
                 window.scrollTo({ top: 0, behavior: 'smooth' });
            }
        } else if (isValid) {
            globalError.classList.add('d-none');
        }

        return isValid;
    }

    // Initialize 7 days explicitly (or load from localStorage)
    initDays();

    // ----------------------------------------------------------------------
    // 5. Darkmode Toggle
    // ----------------------------------------------------------------------
    const themeToggle = document.getElementById('themeToggle');
    const themeIcon = document.getElementById('themeIcon');
    const htmlEl = document.documentElement;
    
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
