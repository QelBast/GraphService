const textField = document.getElementById("content");

function getText() {
    // return Array.from(document.querySelectorAll("p"))
    // .map(e => e.textContent)
    // .join('\n');
    return textField.value
}

function removeEdge(button) {
    // Get the row to be removed
    var row = button.parentNode.parentNode;

    // Remove the row from the table
    row.parentNode.removeChild(row);
}

function queryBackend(payload) {
    // NOT_IMPLEMENTED
    console.log(payload);

    $.ajax({
        url: '/Home/WorkWithInputJson',
        type: 'POST',
        dataType: 'json',
        data: { inputJson: payload } ,
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            window.location.href = response.url;
            console.log(response);
        }
    });
}

function getAllNodes() {

    function GraphNode(text, form, label) {
        this.text = text.trim().toLowerCase();
        this.form = form.trim().toLowerCase();
        this.label = label || null; // Set label to null if not provided
    }

    allText = getText();

    const regexPattern = /{{([\w:]+)}}/g;

    arrayPrima = [...allText.matchAll(regexPattern)].map(e => e[1]);
    arraySecunda = arrayPrima.map(e => new GraphNode(...e.split(":")));
    return arraySecunda;

}

function tableToJson() {
    var table = document.getElementById("edgeTable");
    var rows = table.getElementsByTagName("tr");
    var data = [];

    // Start from index 1 to skip the header row
    for (var i = 1; i < rows.length; i++) {
        var cells = rows[i].getElementsByTagName("td");
        var rowData = {
            to: cells[0].textContent,
            from: cells[1].textContent,
            label: cells[2].textContent,
        };
        data.push(rowData);
    }

    return data
}

function getGlobalState() {
    nodes = getAllNodes();
    edges = tableToJson();
    console.log(nodes);
    console.log(edges);
    return {
        nodes: nodes,
        edges: edges,
        nodeTextNames: nodes.map((e) => e['text']),
        edgeNames: edges.map((e) => e['label']),
    }
}

function getProjectState() {
    return {
        edges: tableToJson(),
        text: getText(),
    }
}

function orderGraph() {
    globalState = getGlobalState();
    return queryBackend({
        nodes: globalState.nodes,
        edges: globalState.edges,
    })
}

function undo() {


    if (undoStack.length > 0) {
        // Pop the previous value from the stack and set it back to the text field
        textField.value = undoStack.pop();
    }
}

function renewLabelDropDowns(valuesArray, selector) {
    console.log(valuesArray);
    // Get the dropdown elements
    document.querySelectorAll(selector).forEach((toDropdown) => {
        // Clear existing options
        toDropdown.innerHTML = "";
        // Add options to the dropdown
        for (var i = 0; i < valuesArray.length; i++) {
            var option = document.createElement("option");
            option.value = valuesArray[i];
            option.text = valuesArray[i];
            toDropdown.add(option);
        }
        // Add the "Custom" option
        var customOption = document.createElement("option");
        customOption.value = "custom";
        customOption.text = "Custom";
        toDropdown.add(customOption);
    })
    return
}

function addNode(targetText) {
    if (targetText) {
        // Store the current value before making changes
        undoStack.push(textField.value);

        // Get the selected form from the dropdown
        const formDropdown = document.getElementById("formDropdown");
        const selectedForm = formDropdown.value;

        // Ask the user for "label"
        const label = prompt("Enter node label (optional):");

        // Replace every occurrence of the selected text with the modified text
        const modifiedText = label ? `{{${targetText}:${selectedForm}:${label}}}` : `{{${targetText}:${selectedForm}}}`;
        textField.value = textField.value.replaceAll(targetText, modifiedText);
        renewLabelDropDowns(getGlobalState()['nodeTextNames'], ".nodeDropdown")
    }
}

function addRowToEdgeTable(from, to, edge) {
    // Get the table body
    var tbody = document.getElementById("edgeTable").getElementsByTagName("tbody")[0];

    // Create a new row
    var newRow = tbody.insertRow();
    newRow.insertCell().textContent = from;
    newRow.insertCell().textContent = to;
    newRow.insertCell().textContent = edge;

    // Add a cell for the "Remove" button
    var removeCell = newRow.insertCell();
    removeCell.innerHTML = '<button onclick="removeEdge(this)">Remove</button>';
}

function addEdge() {
    const fromDropdown = document.getElementById("fromDropdown");
    if (fromDropdown.value === "custom") {
        fromValue = prompt("Enter custom 'from' value:")
        addNode(fromValue)
    } else {
        fromValue = fromDropdown.value
    }

    const toDropdown = document.getElementById("toDropdown");
    if (toDropdown.value === "custom") {
        toValue = prompt("Enter custom 'to' value:")
        addNode(toValue)
    } else {
        toValue = toDropdown.value
    }

    const labelDropdown = document.getElementById("labelDropdown");
    const labelValue = labelDropdown.value === "custom" ? prompt("Enter custom 'label' value:") : labelDropdown.value;

    console.log("Edge created with values:");
    console.log("From:", fromValue);
    console.log("To:", toValue);
    console.log("Label:", labelValue);

    addRowToEdgeTable(fromValue, toValue, labelValue);
    renewLabelDropDowns(getGlobalState()['edgeNames'], ".labelDropdown");
}

function eventListener(event) {
    if (event.ctrlKey && event.key === "z") { // Check if Ctrl+Z is pressed
        undo(); // Perform undo operation
    } else if (event.altKey && event.key === "x") { // Check if Alt+X is pressed
        const textField = document.getElementById("content");
        const selectedText = textField.value.substring(textField.selectionStart, textField.selectionEnd);
        addNode(selectedText);

    } else if (event.altKey && event.key === "c") { // Check if Alt+C is pressed
        addEdge();
    }
}

console.log(getAllNodes())
let undoStack = []; // Stack to keep track of changes
document.addEventListener("keydown", eventListener);

queryBackend({
    'nodes': [{'text':'1', 'form':'rectangle'}], 'edges': [
        {
            "to": "1",
            "from": "3",
            "label": "5"
        }
    ]
})