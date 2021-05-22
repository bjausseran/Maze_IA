<?php

namespace App\Http\Controllers;

use App\Test;
use App\Maze;
use App\NameList;
use App\NameObj;
use App\User;
use Illuminate\Http\Request;
use Faker\Generator as Faker;


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
    public function getNames()
    {
        $mazes = Maze::orderBy('id', 'asc')->select('id', 'name')->get();
        return array('Items' => $mazes);

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

        if(!self::checkIfUserExist($request->user_id))
        {
            return "404 : This user does not exists.";
        }
        if(self::checkIfNameExist($request->name))
        {
            return "403 : This name already exists.";
        }

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

    private function checkIfUserExist(Int $user_id)
    {
        if(User::where('id','=', $user_id)->count() == 0)
        {
            return false;
        }
        else 
        {
            return true;
        }
    }
    private function checkIfNameExist(String $name)
    {
        if(maze::where('name','=', $name)->count() == 0)
        {
            return false;
        }
        else 
        {
            return true;
        }
    }


    public function getJSon(Maze $maze)
    {
        return $maze->composition;
    }
    public function getRandomMaze(String $name, Int $authorId)
    {
        factory(Maze::class, 1)->create();
        $lastMaze = $mazes = Maze::orderBy('id', 'desc')->first();
        $lastMaze->name = $name;
        $lastMaze->user_id = $authorId;
        $lastMaze->save();
        return $lastMaze->composition;
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
        
        if(!self::checkIfUserExist($request->user_id))
        {
            return "404 : This user does not exists.";
        }

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
        $tests = Test::where('maze_id','=', $maze->id);
        foreach($tests as $test)
        {
            $test->delete();
        }

        $return = "tests deleted : " + $tests.count();
        $maze->delete();
        return $return + response()->json();
    }
}
