<?php

namespace App\Http\Controllers;

use App\Test;
use App\Maze;
use Illuminate\Http\Request;
use Carbon;

class TestController extends Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        $tests = Test::paginate();
        return $tests;
    }

    /**
     * Store a newly created resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\Response
     */
    public function store(Request $request)
    {
        
        $inputs = $request->except('_token');
        
        if(!self::checkIfMazeExist($request->maze_id))
        {
            return "404 : This maze does not exists.";
        }

        $test = new Test();
        foreach($inputs as $key => $value) 
        {
            $test->$key = $value;
        }
        $test->test_at = Carbon\Carbon::now();
        $test->save();

        return $test;
    }

    /**
     * Display the specified resource.
     *
     * @param  \App\Test  $test
     * @return \Illuminate\Http\Response
     */
    public function show(Test $test)
    {
        return $test;
    }

    /**
     * Update the specified resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  \App\Test  $test
     * @return \Illuminate\Http\Response
     */
    public function update(Request $request, Test $test)
    {
        $inputs = $request->except('_token', '_method');
        
        if(!self::checkIfMazeExist($request->maze_id))
        {
            return "404 : This maze does not exists.";
        }

        foreach($inputs as $key => $value)
        {
            $test->$key = $value;
        }
        $test->save();
        return $test;
    }
    
    private function checkIfMazeExist(Int $maze_id)
    {
        if(Maze::where('id','=', $maze_id)->count() == 0)
        {
            return false;
        }
        else 
        {
            return true;
        }
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  \App\Test  $test
     * @return \Illuminate\Http\Response
     */
    public function destroy(Test $test)
    {
        $test->delete();
        return response()->json();
    }
}
