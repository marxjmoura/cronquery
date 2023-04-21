const gulp = require('gulp')
const autoprefixer = require('gulp-autoprefixer')
const babelify = require('babelify')
const bro = require('gulp-bro')
const connect = require('gulp-connect')
const htmlmin = require('gulp-htmlmin')
const rename = require("gulp-rename")
const sass = require('gulp-sass')(require('sass'));
const uglify = require('gulp-uglify')
const handlebars = require('./handlebars')

function printError(error) {
  console.log(error)
}

function scss() {
  return gulp.src('www/scss/docs.scss')
    .pipe(sass({ outputStyle: 'compressed' }).on('error', printError))
    .pipe(autoprefixer({ overrideBrowserslist: ['last 2 versions'], cascade: false }))
    .pipe(rename({ extname: '.min.css' }))
    .pipe(gulp.dest('dist'))
}

function js() {
  return gulp.src('www/js/docs.js')
    .pipe(bro({ transform: [babelify.configure({ presets: ['@babel/preset-env'] })] }))
    .pipe(uglify().on('error', printError))
    .pipe(rename({ extname: '.min.js' }))
    .pipe(gulp.dest('dist'))
}

function hbs() {
  return gulp.src(['www/**/_*.hbs', 'www/**/*.hbs'], { base: './www' })
    .pipe(handlebars())
    .pipe(rename({ extname: '.html' }))
    .pipe(htmlmin({ collapseWhitespace: true }).on('error', printError))
    .pipe(gulp.dest('dist'))
}

function fonts() {
  return gulp.src('www/**/*.ttf')
    .pipe(gulp.dest('dist'))
}

function img() {
  return gulp.src(['www/**/*.{svg,jpg,png,ico}', '!www/**/_*.*'])
    .pipe(gulp.dest('dist'))
}

function build(done) {
  gulp.parallel(scss, js, hbs, fonts, img)(done)
}

function watch(done) {
  gulp.watch('www/**/*.scss', gulp.series(scss))
  gulp.watch('www/**/*.js', gulp.series(js))
  gulp.watch('www/**/*.hbs', gulp.series(hbs))
  gulp.watch('www/**/*.{svg,jpg,png,ico}', gulp.series(fonts))
  gulp.watch('www/**/*.ttf', gulp.series(img))

  done()
}

function serve(done) {
  connect.server({ root: 'dist', port: 8888, fallback: 'dist/404.html' })
  connect.serverClose()

  done()
}

function start(done) {
  process.env.NODE_ENV = 'development'
  gulp.series(serve, watch, build)(done)
}

function publish(done) {
  process.env.NODE_ENV = 'production'
  gulp.series(build)(done)
}

exports.start = start
exports.publish = publish
