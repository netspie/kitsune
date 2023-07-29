import { useEffect, useCallback, useRef, Ref, useState } from 'react';

import Box from '@mui/material/Box';
import Link from '@mui/material/Link';
import Fade from '@mui/material/Fade';
import Stack from '@mui/material/Stack';
import Portal from '@mui/material/Portal';
import Grid from '@mui/material/Unstable_Grid2';
import Popover from '@mui/material/Popover';
import Label from 'src/components/label';
import Image from 'src/components/image';
import { usePathname } from 'src/routes/hooks';
import { useBoolean } from 'src/hooks/use-boolean';
import { RouterLink } from 'src/routes/components';
import { useActiveLink } from 'src/routes/hooks/use-active-link';

import { NavListProps, NavItemBaseProps } from '../types';

import { NavItem } from './nav-item';
import { StyledMenu, StyledSubheader } from './styles';

type Bounds = {
  top:number;
  left:number; 
  width:number; 
  height:number;
}

// ----------------------------------------------------------------------

export default function NavList({ item }: { item: NavItemBaseProps }) {
  const pathname = usePathname();
  const navRef = useRef<HTMLDivElement>(null);
  const [popBounds, setPopBounds] = useState<Bounds>({top: 0, left:0, width:0, height: 0});
  const menuOpen = useBoolean();

  const active = useActiveLink(item.path, false);

  const externalLink = item.path.includes('http');

  const mainList = item.children ? item.children.filter((list) => list.subheader !== 'Common') : [];

  const commonList = item.children
    ? item.children.find((list) => list.subheader === 'Common')
    : null;

  useEffect(() => {
    if (menuOpen.value) {
      menuOpen.onFalse();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [pathname]);

  useEffect(() => {
    if (!navRef || !navRef.current)
      return;

   setPopBounds(navRef.current.getBoundingClientRect());

  }, [navRef]);

  const handleOpenMenu = useCallback(() => {
    if (item.children) {
      menuOpen.onTrue();
    }
  }, [item.children, menuOpen]);

  return (
    <>
      <NavItem
        ref={navRef}
        item={item}
        active={active}
        open={menuOpen.value}
        externalLink={externalLink}
        onMouseEnter={menuOpen.onTrue}
        onMouseLeave={menuOpen.onFalse}
      />
      
      {!!item.children && menuOpen.value && (
        <Stack sx={{position: 'absolute', width: 'fit', left: popBounds.left}}>
          <Fade in={menuOpen.value}>
            <StyledMenu onMouseEnter={handleOpenMenu} onMouseLeave={menuOpen.onFalse}
              sx={{ width: 'fit-content'}}>
              <Box
                gap={5}
                display="flex"
                gridTemplateColumns="repeat(5, 1fr)"
                sx={{
                  px: 5,
                  pt: 1,
                  pb: 2,
                  height: 1,
                  position: 'relative',
                  bgcolor: 'background.neutral',
                }}
              >
                {mainList.map((list) => (
                  <NavSubList
                    key={list.subheader}
                    subheader={list.subheader}
                    cover={list.cover}
                    items={list.items}
                    isNew={list.isNew}
                  />
                ))}
              </Box>

              {commonList && (
                <Grid xs={3}>
                  <Box sx={{ bgcolor: 'background.default', p: 5 }}>
                    <NavSubList subheader={commonList.subheader} items={commonList.items} />
                  </Box>
                </Grid>
              )}
            </StyledMenu>
          </Fade>
        </Stack>
      )}
    </>
  );
}

// ----------------------------------------------------------------------

function NavSubList({ subheader, isNew, cover, items }: NavListProps) {
  const pathname = usePathname();

  const coverPath = items.length ? items[0].path : '';

  const commonList = subheader === 'Common';

  return (
    <Stack spacing={2}>
      <StyledSubheader>
        {subheader}
        {isNew && (
          <Label color="info" sx={{ ml: 1 }}>
            NEW
          </Label>
        )}
      </StyledSubheader>

      {/* {!commonList && coverPath && coverPath.length > 0 && (
        <Link component={RouterLink} href={coverPath}>
          <Image
            disabledEffect
            alt={cover}
            src={cover || '/assets/placeholder.svg'}
            ratio="16/9"
            sx={{
              borderRadius: 1,
              cursor: 'pointer',
              boxShadow: (theme) => theme.customShadows.z8,
              transition: (theme) => theme.transitions.create('all'),
              '&:hover': {
                opacity: 0.8,
                boxShadow: (theme) => theme.customShadows.z24,
              },
            }}
          />
        </Link>
      )} */}

      <Stack spacing={1.5} alignItems="flex-start">
        {items.map((item) => {
          const active = pathname === item.path || pathname === `${item.path}/`;

          return <NavItem key={item.title} item={item} active={active} subItem />;
        })}
      </Stack>
    </Stack>
  );
}
