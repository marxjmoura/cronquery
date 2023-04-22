import $ from 'jquery'

const $body = $('body')
const $sidebar = $('.sidebar')

$('.sidebar-toggle').on('click', function (e) {
  $sidebar.scrollTop(0)

  const toggle = !$body.hasClass('sidebar-show')
  $body.toggleClass('sidebar-show', toggle)

  e.preventDefault()
})
