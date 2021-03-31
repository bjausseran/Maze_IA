<?php

use Illuminate\Http\Request;
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

Route::middleware('auth:api')->get('/user', function (Request $request) {
    return $request->user();
});

Route::apiResource('/maze', 'MazeController');
Route::apiResource('/tile', 'TileController');
Route::apiResource('/test', 'TestController');

Route::get('/maze/json/{maze}', 'MazeController@getJson')->name('maze.get_json');
Route::get('/mazelist', 'MazeController@getNames')->name('maze.mazelist');
