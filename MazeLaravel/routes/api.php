<?php

use Illuminate\Http\Request;
use App\Http\Middleware\UserKey;
use App\Http\Middleware\AdminKey;
use Illuminate\Support\Facades\Route;

/*
|--------------------------------------------------------------------------
| API Routes
|--------------------------------------------------------------------------
|
| Here is where you can register API routes for your application. These
| routes are loaded by the RouteServiceProvider within a group which
| is assigned the "api" middleware group. Enjoy building your API!
|
*/

Route::middleware(UserKey::class)->get('/user/unused', function (Request $request) {
    return $request->user();
});


/*--------------------------------------------------------------------------
| Only admins       HEADER : [ADMINKEY]
|--------------------------------------------------------------------------*/
Route::middleware(AdminKey::class)->group(function () {
    Route::apiResource('/tile', 'TileController');
    Route::apiResource('/test', 'TestController')->except('store');
    Route::apiResource('/maze', 'MazeController')->only(['destroy', 'index', 'show']);
    Route::apiResource('/user', 'UserController')->only(['destroy', 'index', 'show', 'update']);
});


/*--------------------------------------------------------------------------
| All users         HEADER : [ADMINKEY || USERKEY]
|--------------------------------------------------------------------------*/
Route::middleware(UserKey::class)->group(function () {
    Route::apiResource('/maze', 'MazeController')->only(['store', 'update']);
    Route::get('/maze/json/{maze}', 'MazeController@getJson')->name('maze.get_json');
    Route::get('/mazelist', 'MazeController@getNames')->name('maze.mazelist');
    Route::get('/randommaze/{name}/{authorid}', 'MazeController@getRandomMaze')->name('maze.get_random');
    Route::apiResource('/test', 'TestController')->only('store');
});

/*--------------------------------------------------------------------------
| Everybody         NO HEADER
|--------------------------------------------------------------------------*/
Route::apiResource('/user', 'UserController')->only('store');
Route::post('/user/login', 'UserController@login')->name('user.login');
// here for test, should be deleted before export
Route::get('/test', function(){




});