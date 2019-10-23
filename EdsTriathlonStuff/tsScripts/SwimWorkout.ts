﻿$(function () {
    var poolLength: number = 25;
    var yards: boolean = true;
    var dialogTitle: string = "";
    var workoutId: string = "";

    $(".poollengthinput").on("change", function (event: Event) {
        var parts = (<HTMLInputElement>event.currentTarget).value.split(' ');
        poolLength = parseInt(parts[0]);
        if (poolLength === 33) {
            poolLength = 33.33333;

        }

        yards = parts[1] == "Yards";
        UpdateYardage(true);
    });

    $(".addvalue").on("click", function(event: Event): boolean {
        var index: string = (<HTMLInputElement>event.currentTarget).getAttribute("data_index");
        var newitem: string = <string>$("#newOptionTxt" + index).val(); //get the new value

        var newInt: number = parseInt(newitem);
        var mod: number = newInt % poolLength
        if (mod > 1 && poolLength - mod > 1) {
            alert("Not legit for pool size");
            return true;
        }

        var selectLists: HTMLCollectionOf<HTMLSelectElement> =
            (<HTMLCollectionOf<HTMLSelectElement>>document.getElementsByClassName("distselect"));
        for (var i = 0; i < selectLists.length; i++) {
            var newOption: HTMLOptionElement = <HTMLOptionElement>window.document.createElement("OPTION");
            newOption.text = newitem;
            newOption.value = newitem;

            for (var j = 0; j < selectLists[i].options.length; j++) {
                if (newInt == parseInt(selectLists[i].options[j].value)) {
                    break;
                }

                if (parseInt(newOption.value) < parseInt(selectLists[i].options[j].value) ||
                    selectLists[i].options[j].text == "Other") {
                    selectLists[i].options.add(newOption, selectLists[i].options[j]);
                    break;
                }
            }
        }

        $("#div_content" + index).css("display", "none");
        var distId = "dist" + index;
        (<HTMLInputElement>document.getElementById(distId)).value = newOption.value;
        return false
    });

    $(".poollengthinput").on("change", function (event) {
        var parts = (<HTMLInputElement>event.currentTarget).value.split(' ');
        poolLength = parseInt(parts[0]);
        if (poolLength === 33) {
            poolLength = 33.33333;

        }

        yards = parts[1] == "Yards";
        UpdateYardage(true);
    });

    $("#print").click(function printSwimWorkout() {
        var headstr: string = "<html><head><title></title></head><body>";
        var footstr: string = "</body>";
        var newstr: string = (<HTMLElement>document.all.item("swimout")).innerHTML;
        console.log("%O", document.all.item("swimout"));
        console.log("%O", newstr);

        var oldstr: string = document.body.innerHTML;
        document.body.innerHTML = headstr + newstr + footstr;
        window.print();
        document.body.innerHTML = oldstr;
        return false;
    });

    $(".repsentry").on("change", function (event) {
        var val: string  = (<HTMLInputElement>event.currentTarget).value;
        var i: number = parseInt(event.currentTarget.getAttribute("data-index"));
        UpdateSet(i);
        UpdateYardage(false);
    });

    $(".distselect").change(function (event) {
        var value = (<HTMLInputElement>event.currentTarget).value; //get the selected option value
        var index: number = parseInt(event.currentTarget.getAttribute("data-index"));
        if (value === "other") {
            $("#div_content" + index).css("display", "block"); //display the add new dialog
        } else {
            UpdateSet(index);
            UpdateYardage(false);
        }
    });

    $("#dialog-1").dialog({
        autoOpen: false
    });

    $(".btn.btn-primary").contextmenu(function (e) {
        var rect: ClientRect = e.currentTarget.getBoundingClientRect();
        dialogTitle = e.currentTarget.innerText;
        workoutId = e.currentTarget.getAttribute("data_workoutid");
        $("#dialog-1").dialog({
            title: e.currentTarget.innerText,
            position: {
                my: "left top",
                at: "right bottom",
                of: "#" + e.currentTarget.id
            }
        });
        $("#dialog-1").dialog("open");
        console.log("%O", e);
        e.preventDefault();
    });

    $("#delete").click(function (e) {
        $.post("CreateWorkout/DeleteWorkout",
            { Name: dialogTitle, Id: workoutId },
            function (data) {
                if (data == "success") {
                    location.reload();
                }
            });
    });

    $("#rename").click(function (e) {
        var newName: string = (<HTMLInputElement>$("#newname")[0]).value;
        $.post("CreateWorkout/RenameWorkout",
            { Name: newName, Id: workoutId },
            function (data) {
                if (data == "success") {
                    location.reload();
                }
            });
    });

    function UpdateSet(i: number) {
        var repsId: string = "reps" + i;
        var distId = "dist" + i;
        var totId = "tot" + i;
        var repsCount = parseInt((<HTMLInputElement>document.getElementById(repsId)).value);
        var dist = parseFloat((<HTMLInputElement>document.getElementById(distId)).value);
        var tot = repsCount * dist;
        (<HTMLInputElement>document.getElementById(totId)).value = Math.round(tot).toString();
    }

    function UpdateYardage(poolSizeChanged: boolean) {
        var index = 0;
        if (poolSizeChanged) {
            var stdSetLengths = [1, 2, 3, 4, 5, 6, 8, 12, 16, 20, 32, 40];
            var selectLists: HTMLCollectionOf<HTMLSelectElement> =
                (<HTMLCollectionOf<HTMLSelectElement>>document.getElementsByClassName("distselect"));
            for (var i = 0; i < selectLists.length; i++) {
                selectLists[i].options.length = 0;

                stdSetLengths.forEach(function (lenghts) {
                    var newOption: HTMLOptionElement =
                        <HTMLOptionElement>window.document.createElement("OPTION");
                    var newItem = lenghts * poolLength;
                    newOption.text = Math.round(newItem).toString();
                    newOption.value = newItem.toString();
                    selectLists[i].options.add(newOption);
                });

                var newOption: HTMLOptionElement =
                    <HTMLOptionElement>window.document.createElement("OPTION");
                newOption.text = "Other";
                newOption.value = "Other";
                selectLists[i].options.add(newOption);

            };
        }

        index = 0;
        var total: number = 0;
        var totId: string = "tot" + index;
        var dist: HTMLInputElement = <HTMLInputElement>document.getElementById(totId);
        do {
            total += parseInt(dist.value, 10);
            totId = "tot" + ++index;
            dist = <HTMLInputElement>document.getElementById(totId);
        }
        while (dist != null)

        if (yards) {
            (<HTMLInputElement>document.getElementById("yardstotal")).value =
                Math.round(total).toString();
            (<HTMLInputElement>document.getElementById("meterstotal")).value =
                Math.round(0.9144 * total).toString();
        }
        else {
            (<HTMLInputElement>document.getElementById("meterstotal")).value =
                Math.round(total).toString();
            (<HTMLInputElement>document.getElementById("yardstotal")).value =
                Math.round(1.0936 * total).toString();
        }
    }
});