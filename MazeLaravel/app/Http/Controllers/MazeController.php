<?php

namespace App\Http\Controllers;

use App\Maze;
use Illuminate\Http\Request;

class MazeController extends Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        $mazes = Maze::paginate();
        return $mazes;
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
        $maze = new Maze();
        foreach($inputs as $key => $value) 
        {
            $maze->$key = $value;
        }
        $maze->save();

        return $maze;
    }

    /**
     * Display the specified resource.
     *
     * @param  \App\Maze  $maze
     * @return \Illuminate\Http\Response
     */
    public function show(Maze $maze)
    {
        return $maze;
    }
    public function findJSon(Maze $maze)
    {
        return $maze->$composition;
    }

    /**
     * Update the specified resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  \App\Maze  $maze
     * @return \Illuminate\Http\Response
     */
    public function update(Request $request, Maze $maze)
    {
        $inputs = $request->except('_token', '_method');
        foreach($inputs as $key => $value)
        {
            $maze->$key = $value;
        }
        $maze->save();
        return $maze;
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  \App\Maze  $maze
     * @return \Illuminate\Http\Response
     */
    public function destroy(Maze $maze)
    {
        $maze->delete();
        return response()->json();
    }
}
