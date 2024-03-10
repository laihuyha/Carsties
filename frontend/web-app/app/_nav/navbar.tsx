import { getCurrentUser } from "../_actions/auth-actions";
import LoginButton from "../_components/LoginButton";
import DropdownUserAction from "./dropdown-user-action";
import Logo from "./logo";
import Search from "./search";

const Navbar = async () => {
  const user = await getCurrentUser();
  return (
    <header className="sticky top-0 z-50 flex justify-between p-5 items-center text-gray-800 shadow-md bg-white">
      <Logo />
      <Search />
      {user ? <DropdownUserAction user={user} /> : <LoginButton />}
    </header>
  );
};

export default Navbar;
