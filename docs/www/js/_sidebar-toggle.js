import $ from 'jquery'

const $body = $('body')
const $nav = $('nav')

$('.sidebar-toggle').on('click', function (e) {
  $nav.scrollTop(0)

  const toggle = !$body.hasClass('sidebar-show')
  $body.toggleClass('sidebar-show', toggle)

  e.preventDefault()
})
