$.fn.dataTable.ext.classes.sLengthSelect = 'form-control form-control-sm select';
$.fn.dataTable.ext.classes.sPageButton = 'page-item';

$.extend($.fn.dataTable.defaults, {
	language: {
		searchPlaceholder: "Search records",
		search: "",
		paginate: {
			previous: '<i class="la la-angle-left"></i>',
			next: '<i class="la la-angle-right"></i>'
		}
	},
	responsive: true
});

$(document).ready(function () {
    $('.select').select2({
        //width: '100%',
        minimumResultsForSearch: 10,
        placeholder: 'Choose One'
	});

    $('.select-multiple').select2({
        width: '100%',
        minimumResultsForSearch: 10,
        placeholder: 'Choose Items',
		multiple: true,
		allowClear: true,
		closeOnSelect: false
    });

	ClassicEditor
		.create(document.querySelector('.editor'), {
			height: 500,
			toolbar: {
				items: [
					'heading',
					'|',
					'bold',
					'italic',
					'underline',
					'link',
					'bulletedList',
					'numberedList',
					'|',
					'fontFamily',
					'fontSize',
					'fontColor',
					'fontBackgroundColor',
					'highlight',
					'|',
					'alignment',
					'indent',
					'outdent',
					'|',
					'imageUpload',
					'blockQuote',
					'insertTable',
					'mediaEmbed',
					'undo',
					'redo',
					'removeFormat',
					'horizontalLine'
				]
			},
			language: 'en',
			image: {
				toolbar: ['imageTextAlternative', '|', 'imageStyle:alignLeft', 'imageStyle:full', 'imageStyle:alignRight'],
				styles: [
					// This option is equal to a situation where no style is applied.
					'full',

					// This represents an image aligned to the left.
					'alignLeft',

					// This represents an image aligned to the right.
					'alignRight'
				]
			},
			table: {
				contentToolbar: [
					'tableColumn',
					'tableRow',
					'mergeTableCells'
				]
			},
			link: {
				addTargetToExternalLinks: true,
				decorators: [
					{
						mode: 'manual',
						label: 'Downloadable',
						attributes: {
							download: 'download'
						}
					}
				]
			},
			licenseKey: '',

		})
		.then(editor => {
			window.editor = editor;




		})
		.catch(error => {
			console.error(error);
		});


    $('[data-toggle="popover"]').popover({
        trigger: 'focus',
        animation: true
    });

    $('[data-toggle="popover-html"]').popover({
        trigger: 'focus',
        html: true,
        animation: true
    });

    $('.az-toggle').on('click', function () {
        $(this).toggleClass('on');
        var el = $(this).find('input[type="checkbox"]');
        var state = el.attr('checked');
        $(el).attr('checked', !state);
        el.trigger("change");
	});


/*Data Table*/

	$('.datatable.table-2').DataTable({
	});

	$('.datatable:not(.table-2)').DataTable({
		paging: false,
		searching: false,
		language: {
			searchPlaceholder: "Search records",
			search: ""
		},
		pageLength: 50,
		buttons: ['copy', 'csv', 'excel'],
		info: false
	});

/* Fancybox */

	$('[data-fancybox]').fancybox({
		touch: false,
	});

	$('#table thead th:not(.not-sort)').on('click', function () {
		SortTable(this);
	});
});

function checkAll(el) {
	var i;
	let items = document.getElementsByClassName("bulk-select");
	for (i = 0; i < items.length; i++) {
		items[i].checked = el.checked;
	}
}

function SortTable(el) {
	lastSortedColumn = -1;
	var table, rows, col, cols, switching, i, x, y, shouldSwitch, desc, switchCount = 0;
	table = document.getElementById("table");

	cols = Array.prototype.slice.call(table.getElementsByTagName('thead')[0].getElementsByTagName("tr")[0].children);
	col = cols.indexOf(el);

	switching = true;
	/* Make a loop that will continue until
	no switching has been done: */
	while (switching) {
		// Start by saying: no switching is done:
		switching = false;
		rows = table.rows;
		/* Loop through all table rows (except the
		first, which contains table headers): */
		for (i = 1; i < (rows.length - 1); i++) {
			// Start by saying there should be no switching:
			shouldSwitch = false;
			/* Get the two elements you want to compare,
			one from current row and one from the next: */
			x = rows[i].children[col];
			y = rows[i + 1].children[col];
			// Check if the two rows should switch place:

			if (desc) {
				if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {
					// If so, mark as a switch and break the loop:
					shouldSwitch = true;
					break;
				}
			} else {
				if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {
					// If so, mark as a switch and break the loop:
					shouldSwitch = true;
					break;
				}
			}
		}

		if (shouldSwitch) {
			/* If a switch has been marked, make the switch
			and mark that a switch has been done: */
			rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
			switching = true;
			switchCount++;
		} else {
			if (switchCount == 0) {
				desc = true;
				switching = true;
			}
		}
	}	
}


function FilterTable(i, id) {
	// Declare variables
	var td, i, j, txtValue;
	let input = document.getElementById(i);
	let filter = input.value.toUpperCase();
	let table = document.getElementById(id);
	let body = table.getElementsByTagName("tbody")[0];
	let tr = body.getElementsByTagName("tr");


	// Loop through all table rows, and hide those who don't match the search query
	for (i = 0; i < tr.length; i++) {
		td = tr[i].getElementsByTagName("td");
		let found = false;
		for (j = 0; j < td.length; j++) {
			if (td[j]) {
				txtValue = td[j].textContent || td[j].innerText;
				if (txtValue.toUpperCase().indexOf(filter) > -1) {
					found = true;
					break;
				} else {
					found = false;
				}
			}
		}

		if (found) {
			tr[i].style.display = "";
		}
		else {
			tr[i].style.display = "none";
		}
	}
}