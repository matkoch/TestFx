
function completeData(data) {
  data.icon = getIconClass(data.state);
  delete data.state;

  var nodes = [];
  if (data.hasOwnProperty('suites')) nodes = nodes.concat(data.suites);
  if (data.hasOwnProperty('tests')) nodes = nodes.concat(data.tests);

  if (nodes.length > 0) {
    nodes.forEach(completeData);
    data.nodes = nodes;
  }
}

function selectNode(nodeId) {
  for (var node of $('#treeview1').treeview('getNodes')) {
    if (node.nodeId === nodeId) {
      $('#treeview1').treeview('selectNode', node);
      return;
    }
  }
}

function getIconClass(state) {
  switch (state) {
    case 'Passed': return 'glyphicon glyphicon-ok passed';
    case 'Failed': return 'glyphicon glyphicon-remove failed';
    case 'Inconclusive': return 'glyphicon glyphicon-warning-sign inconclusive';
    case 'Ignored': return 'glyphicon glyphicon-eye-open ignored';
  }
}

function createTableBody(operations) {
  var getColorClass =
    function getColorClass(operation) {
      switch (operation.state) {
        case 'Passed': return operation.type === 'Assertion' ? 'success' : '';
        case 'Failed': return 'danger';
        case 'Inconclusive': return 'warning';
        case 'Ignored': return 'info';
      }
    }

  var content = '';
  for (var i = 0; i < operations.length; i++) {
    var operation = operations[i];
    if (operation.id === 'separator') {
      content += '<tr><td class="text-center">...</td><td><em>Inner operations</em></td></tr>';
      continue;
    }
    content += '<tr class="' + getColorClass(operation) + '">';
    content += '  <td class="text-center">' + (i+1) + '</td>';
    content += '  <td>' + operation.text + '</td>';
    content += '</tr>';
  }
  return content;
}

function getNodeHierarchy(node) {
  var getParentChain =
    function*(node) {
      while (node) {
        yield node;
        node = $('#treeview1').treeview('getParents', node)[0];
      }
    };
  return Array.from(getParentChain(node)).reverse();
}

function createBreadcrumb(node) {
  var content = '<li><a href="#"><span class="glyphicon glyphicon-home"></span></a></li>';
  var hierarchy = getNodeHierarchy(node);
  for (var i = 0; i < hierarchy.length; i++) {
    var node2 = hierarchy[i];
    if (i + 1 == hierarchy.length)
      content += '<li class="active">' + node2.text + '</li>';
    else
      content += '<li><a onclick="selectNode(\'' + node2.nodeId + '\')">' + node2.text + '</a></li>';
  }
  return content;
}

function onNodeSelected(event, node) {
  if (node.hasOwnProperty('operations'))
    $('#operations-table tbody').html(createTableBody(node.operations));
  else if (node.hasOwnProperty('setups')) {
    $('#operations-table tbody').html(createTableBody(
      node.setups.concat({ id: 'separator'}).concat(node.cleanups)));
  }
  else
    $('#operations-table tbody').html('<tr><td></td><td><em>No operations</em></td></tr>');

  //$('#treeview1').treeview('getParent', node)
  $('.antiscroll-wrap').antiscroll();
  $('#breadcrumb').html(createBreadcrumb(node));
}

$(function() {


  $.getJSON('./resultData.json',
    function(data){

      completeData(data);

      $('#treeview1').treeview({
        data: data.nodes,
        showBorder: false,
        expandIcon: 'glyphicon glyphicon-triangle-right',
        collapseIcon: 'glyphicon glyphicon-triangle-bottom',
        onNodeSelected: onNodeSelected
      });

      $('.antiscroll-wrap').antiscroll();
    });


  $('#filter input').click(function() {
      $(this).closest('label').toggleClass('active');
  });



  $('.btn-toggle').click(function() {
      $(this).find('.btn').toggleClass('active').blur();
  });

});
$(function () {
  var data: CircularChartData[] = [
    { value: 11, color: '#449d44', label: 'Passed' },
    { value: 11, color: '#c9302c', label: 'Failed' },
    { value: 11, color: '#ec971f', label: 'Inconclusive' },
    { value: 11, color: '#31b0d5', label: 'Ignored' }
  ];


  //var canvas = document.getElementById('chart-canvas') as HTMLCanvasElement;
  //var ctx = canvas.getContext('2d');
  //var myDoughnutChart = new Chart(ctx).Doughnut(data);
});

// resizable https://codepen.io/barbalex/pen/ogZWNV
   